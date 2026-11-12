#include <Wire.h>
#include <WiFi.h>
#include <WiFiClient.h>
#include <HTTPClient.h>
#include <ArduinoJson.h>
#include "MAX30105.h"
#include "spo2_algorithm.h"

// ============================================================================
// 1. إعدادات شبكة الـ Wi-Fi
// ============================================================================
const char* WIFI_SSID     = "Eng Moaz";
const char* WIFI_PASSWORD = "Moaz2004";

// ============================================================================
// 2. إعدادات الـ Backend API
// ============================================================================
const char* API_HOST = "23.8.8.105";
const uint16_t API_PORT = 5008;

// Endpoints
const char* API_GET_CURRENT_PATIENT_URL = "http://23.8.8.105:5008/api/current-patient";
const char* API_POST_MEASUREMENT_URL    = "http://23.8.8.105:5008/api/Heart-Rate";

int PatientId = 0;            // المعرف الخاص بالمريض النشط المختار من واجهة الطبيب/الممرضة
String SelectionVersion = ""; // رقم نسخة الجلسة لمنع إرسال قراءات متأخرة بعد تبديل المريض

// ============================================================================
// 3. إعدادات حساس MAX30102
// ============================================================================
#define SDA_PIN 10
#define SCL_PIN 9

MAX30105 particleSensor;

// 100 عينة = 4 ثوانٍ بمعدل أخذ عينات 25 عينة/ثانية (لرسم المنحنى بدقة)
const int BUFFER_LENGTH = 100;

// الحد الأدنى لقيمة الـ IR لاكتشاف وجود الإصبع
const uint32_t FINGER_THRESHOLD = 30000;

uint32_t irBuffer[BUFFER_LENGTH];
uint32_t redBuffer[BUFFER_LENGTH];

int32_t spo2;
int8_t validSPO2;

int32_t heartRate;
int8_t validHeartRate;

bool fingerWasDetected    = false; // متابعة حالة الإصبع
bool standbyPromptPrinted = false; // لطباعة رسالة الانتظار مرة واحدة فقط
bool bufferInitialized    = false; // هل تم جمع الـ 100 عينة الأولى بنجاح؟

// ============================================================================
// 4. التوقيتات وإدارة الفترات الزمنية
// ============================================================================
unsigned long lastSendTime = 0;
const unsigned long SEND_INTERVAL = 3000; // فاصل 3 ثوانٍ بين كل إرسال ناجح للداتابيز

unsigned long lastPatientCheckTime = 0;
const unsigned long PATIENT_CHECK_INTERVAL = 3000; // فحص المريض المختار كل 3 ثوانٍ

// ============================================================================
// دالة الاتصال بالواي فاي
// ============================================================================
void connectWiFi()
{
    Serial.println();
    Serial.println("========== WIFI CONNECTION ==========");

    WiFi.mode(WIFI_STA);
    WiFi.begin(WIFI_SSID, WIFI_PASSWORD);

    Serial.print("Connecting to Wi-Fi");

    int attempts = 0;
    while (WiFi.status() != WL_CONNECTED && attempts < 30)
    {
        delay(500);
        Serial.print(".");
        attempts++;
    }

    Serial.println();

    if (WiFi.status() == WL_CONNECTED)
    {
        Serial.println("[Wi-Fi] Connected successfully!");
        Serial.print("[Wi-Fi] ESP32 IP: ");
        Serial.println(WiFi.localIP());
    }
    else
    {
        Serial.println("[Wi-Fi] Connection FAILED! Retrying soon...");
    }

    Serial.println("=====================================");
}

// ============================================================================
// تصفير جلسة القياس الحالية عند تغيير المريض لمنع اختلاط العينات
// ============================================================================
void resetMeasurementSession()
{
    bufferInitialized    = false;
    fingerWasDetected    = false;
    standbyPromptPrinted = false;
    particleSensor.clearFIFO();
}

// ============================================================================
// فحص المريض المختار حالياً من الـ Backend
// ============================================================================
bool fetchCurrentPatient()
{
    if (WiFi.status() != WL_CONNECTED)
    {
        return false;
    }

    WiFiClient client;
    HTTPClient http;

    http.setConnectTimeout(3000);
    http.setTimeout(5000);

    if (!http.begin(client, API_GET_CURRENT_PATIENT_URL))
    {
        Serial.println("[API] Failed to initialize HTTP client for current-patient.");
        return false;
    }

    int httpCode = http.GET();

    if (httpCode == 200)
    {
        String payload = http.getString();

        JsonDocument doc;
        DeserializationError error = deserializeJson(doc, payload);

        if (!error)
        {
            int receivedPatientId = doc["patientId"] | 0;
            String receivedVersion = doc["selectionVersion"] | "";

            if (receivedPatientId != PatientId || receivedVersion != SelectionVersion)
            {
                PatientId = receivedPatientId;
                SelectionVersion = receivedVersion;
                resetMeasurementSession();
                Serial.println();
                Serial.println(">>> [PATIENT SELECTED] Active Patient ID: " + String(PatientId) + " (Version: " + SelectionVersion + ")");
            }

            http.end();
            return true;
        }
        else
        {
            Serial.print("[API] JSON Deserialization error: ");
            Serial.println(error.c_str());
        }
    }
    else if (httpCode == 404)
    {
        if (PatientId != 0 || SelectionVersion.length() > 0)
        {
            Serial.println("[API] No patient is currently selected on backend.");
            resetMeasurementSession();
        }
        PatientId = 0;
        SelectionVersion = "";
    }
    else
    {
        Serial.print("[API] GET current-patient failed, HTTP Code: ");
        Serial.println(httpCode);
        if (httpCode < 0)
        {
            Serial.println(http.errorToString(httpCode));
        }
    }

    http.end();
    return false;
}

// ============================================================================
// تهيئة حساس MAX30102
// ============================================================================
bool initializeSensor()
{
    Serial.println();
    Serial.println("========== SENSOR INIT ==========");

    Wire.begin(SDA_PIN, SCL_PIN);

    if (!particleSensor.begin(Wire, I2C_SPEED_FAST))
    {
        Serial.println("[ERROR] MAX30102 was not found! Check wiring (SDA/SCL/VCC/GND).");
        return false;
    }

    Serial.println("[OK] MAX30102 detected successfully!");

    byte ledBrightness = 60;  // إضاءة متوازنة
    byte sampleAverage = 4;   // تجميع كل 4 عينات لتقليل النويز
    byte ledMode = 2;         // تشغيل Red + IR معاً
    int sampleRate = 100;     // 100 عينة في الثانية
    int pulseWidth = 411;     // دقة كاملة 18-bit للـ ADC
    int adcRange = 4096;      // المدى الكامل للـ ADC

    particleSensor.setup(
        ledBrightness,
        sampleAverage,
        ledMode,
        sampleRate,
        pulseWidth,
        adcRange
    );

    // تيار إضاءة قوي ومستقر
    particleSensor.setPulseAmplitudeRed(0x2A);
    particleSensor.setPulseAmplitudeIR(0x2A);
    particleSensor.setPulseAmplitudeGreen(0);

    Serial.println("[OK] Sensor initialized with optimal medical settings.");
    Serial.println("=================================");

    return true;
}

// ============================================================================
// فحص خفيف وسريع لوجود الإصبع (Non-blocking)
// ============================================================================
bool isFingerDetected()
{
    long ir = particleSensor.getIR();
    return (ir >= FINGER_THRESHOLD);
}

// ============================================================================
// معالجة عينات الحساس بنظام النافذة المنزلقة (Sliding Window) للثبات والاستقرار
// ============================================================================
bool processSensorReadings()
{
    // المرحلة 1: إذا كانت هذه بداية وضع الإصبع، نجمع أول 100 عينة (4 ثوانٍ)
    if (!bufferInitialized)
    {
        Serial.print("[SENSOR] Calibrating pulse baseline (4 sec)... [");

        for (int i = 0; i < BUFFER_LENGTH; i++)
        {
            unsigned long startWaitingTime = millis();

            while (!particleSensor.available())
            {
                particleSensor.check();
                if (millis() - startWaitingTime > 2000)
                {
                    Serial.println(" Timeout!]");
                    return false;
                }
                delay(1);
            }

            redBuffer[i] = particleSensor.getRed();
            irBuffer[i]  = particleSensor.getIR();
            particleSensor.nextSample();

            if ((i + 1) % 20 == 0)
            {
                Serial.print("=");
            }

            // إذا رفع المستخدم إصبعه أثناء المعايرة
            if (irBuffer[i] < FINGER_THRESHOLD)
            {
                Serial.println(" Aborted!]");
                bufferInitialized = false;
                fingerWasDetected = false;
                return false;
            }
        }

        Serial.println("] Done!");
        bufferInitialized = true;
    }
    else
    {
        // المرحلة 2: نافذة منزلقة (نحتفظ بـ 75 عينة سابقة ونأخذ 25 عينة جديدة فقط = ثانية واحدة)
        for (int i = 25; i < BUFFER_LENGTH; i++)
        {
            redBuffer[i - 25] = redBuffer[i];
            irBuffer[i - 25]  = irBuffer[i];
        }

        for (int i = 75; i < BUFFER_LENGTH; i++)
        {
            unsigned long startWaitingTime = millis();

            while (!particleSensor.available())
            {
                particleSensor.check();
                if (millis() - startWaitingTime > 2000)
                {
                    return false;
                }
                delay(1);
            }

            redBuffer[i] = particleSensor.getRed();
            irBuffer[i]  = particleSensor.getIR();
            particleSensor.nextSample();

            if (irBuffer[i] < FINGER_THRESHOLD)
            {
                Serial.println("\n[SENSOR] Finger removed.");
                bufferInitialized = false;
                fingerWasDetected = false;
                return false;
            }
        }
    }

    // حساب النبض والأكسجين بواسطة خوارزمية Maxim
    maxim_heart_rate_and_oxygen_saturation(
        irBuffer,
        BUFFER_LENGTH,
        redBuffer,
        &spo2,
        &validSPO2,
        &heartRate,
        &validHeartRate
    );

    // تصحيح مشكلة العد المزدوج (Dicrotic Notch)
    if (heartRate >= 120 && (heartRate / 2) >= 50 && (heartRate / 2) <= 100)
    {
        heartRate = heartRate / 2;
    }

    // طباعة حية للتقدم لمعرفة حالة الاستقرار
    Serial.print("[LIVE] Heart Rate: " + String(heartRate) + " BPM (" + (validHeartRate ? "Valid" : "Stabilizing...") + ") | ");
    Serial.println("SpO2: " + String(spo2) + "% (" + (validSPO2 ? "Valid" : "Stabilizing...") + ")");

    // إذا كانت القراءة معتمدة ومطابقة للنطاق الطبيعي السليم
    bool hrValidFinal  = (validHeartRate == 1 && heartRate >= 50 && heartRate <= 180);
    bool spo2ValidFinal = (validSPO2 == 1 && spo2 >= 85 && spo2 <= 100);

    // إذا كانت القراءتان صالحتين، تصبح القراءة معتمدة بنجاح
    if (hrValidFinal && spo2ValidFinal)
    {
        Serial.println();
        Serial.println("*************************************************");
        Serial.println(">>> [ACCURATE READING] Validated Measurement <<<");
        Serial.println("   Heart Rate : " + String(heartRate) + " BPM");
        Serial.println("   SpO2       : " + String(spo2) + " %");
        Serial.println("*************************************************");
        return true;
    }

    // في حال كانت نسبة الأكسجين مؤكدة 100% والنبض في نطاق منطقي طبيعي، نعتمدها أيضاً
    if (spo2ValidFinal && heartRate >= 55 && heartRate <= 110)
    {
        Serial.println();
        Serial.println("*************************************************");
        Serial.println(">>> [ACCURATE READING] Stabilized Measurement <<<");
        Serial.println("   Heart Rate : " + String(heartRate) + " BPM");
        Serial.println("   SpO2       : " + String(spo2) + " %");
        Serial.println("*************************************************");
        return true;
    }

    return false;
}

// ============================================================================
// إرسال القراءة المعتمدة لقاعدة البيانات عبر الـ API
// ============================================================================
bool sendDataToAPI()
{
    if (WiFi.status() != WL_CONNECTED)
    {
        Serial.println("[API] Wi-Fi disconnected. Cannot send data.");
        return false;
    }

    if (PatientId <= 0)
    {
        Serial.println("[API] Cannot send: No patient is selected.");
        return false;
    }

    WiFiClient client;
    HTTPClient http;

    http.setConnectTimeout(3000);
    http.setTimeout(7000);

    if (!http.begin(client, API_POST_MEASUREMENT_URL))
    {
        Serial.println("[API] Failed to connect to server endpoint.");
        return false;
    }

    http.addHeader("Content-Type", "application/json");

    // إنشاء الـ JSON للقيمة المعتمدة فقط
    JsonDocument doc;
    doc["patientId"] = PatientId;
    if (SelectionVersion.length() > 0)
    {
        doc["selectionVersion"] = SelectionVersion;
    }
    doc["heartRate"] = heartRate;
    doc["spO2"]      = spo2;

    String jsonPayload;
    serializeJson(doc, jsonPayload);

    Serial.println("[API] Sending Payload to Database: " + jsonPayload);

    int httpResponseCode = http.POST(jsonPayload);

    if (httpResponseCode >= 200 && httpResponseCode < 300)
    {
        Serial.println("[API] Success! Saved to Database. (HTTP " + String(httpResponseCode) + ")");
        http.end();
        return true;
    }
    else
    {
        Serial.print("[API] Failed to save! HTTP Code: ");
        Serial.println(httpResponseCode);
        if (httpResponseCode > 0)
        {
            Serial.println("[API] Server Response: " + http.getString());
        }

        http.end();

        if (httpResponseCode == 400 || httpResponseCode == 409)
        {
            PatientId = 0; // إعادة تعيين في حال تغير المريض أو ألغيت جلسة المريض
            SelectionVersion = "";
            resetMeasurementSession();
        }

        return false;
    }
}

// ============================================================================
// SETUP
// ============================================================================
void setup()
{
    Serial.begin(115200);
    delay(1000);

    Serial.println();
    Serial.println("==============================================");
    Serial.println("  Smart Health Monitoring System (ESP32)");
    Serial.println("  Precision Pulse & Oximeter Sensor Reader");
    Serial.println("==============================================");

    // 1. الاتصال بالـ Wi-Fi
    connectWiFi();

    // 2. تهيئة الحساس
    if (!initializeSensor())
    {
        Serial.println("[FATAL] Sensor setup failed. Check wiring and restart.");
        while (1) { delay(1000); }
    }

    // 3. فحص المريض المختار لأول مرة
    Serial.println("[INFO] Checking for selected patient on backend...");
    fetchCurrentPatient();

    Serial.println("[READY] System initialized and ready.");
    Serial.println("==============================================");
}

// ============================================================================
// LOOP
// ============================================================================
void loop()
{
    // 1. إعادة الاتصال بالـ Wi-Fi تلقائياً إذا انقطع
    if (WiFi.status() != WL_CONNECTED)
    {
        Serial.println("[Wi-Fi] Lost connection! Reconnecting...");
        connectWiFi();
        if (WiFi.status() != WL_CONNECTED)
        {
            delay(3000);
            return;
        }
    }

    unsigned long currentTime = millis();

    // 2. فحص دوري للمريض المختار في الـ Backend كل 3 ثوانٍ
    if (currentTime - lastPatientCheckTime >= PATIENT_CHECK_INTERVAL)
    {
        lastPatientCheckTime = currentTime;
        fetchCurrentPatient();
    }

    // 3. إذا لم يقم الطبيب/الممرضة باختيار مريض، لا تبدأ القياس
    static bool waitPatientPrinted = false;
    if (PatientId <= 0)
    {
        if (!waitPatientPrinted)
        {
            Serial.println("[STANDBY] Waiting for a patient to be selected on backend...");
            waitPatientPrinted = true;
        }
        delay(500);
        return;
    }
    waitPatientPrinted = false;

    // 4. فحص سريع لوجود الإصبع (Non-blocking)
    if (!isFingerDetected())
    {
        if (fingerWasDetected)
        {
            Serial.println("\n[STANDBY] Finger removed. Measurement paused.");
            fingerWasDetected = false;
            bufferInitialized = false;    // تصفير الـ buffer لتبدأ معايرة جديدة عند وضع الإصبع مجدداً
            standbyPromptPrinted = false;
        }

        // طباعة رسالة الانتظار مرة واحدة فقط وتظل الشاشة صامتة تماماً
        if (!standbyPromptPrinted)
        {
            Serial.println("[STANDBY] Place finger on sensor to begin measuring...");
            standbyPromptPrinted = true;
        }

        delay(150);
        return;
    }

    // تم وضع الإصبع للتو!
    if (!fingerWasDetected)
    {
        Serial.println("\n>>> [SENSOR] Finger detected! Initializing clean buffer...");
        fingerWasDetected = true;
        bufferInitialized = false;
        standbyPromptPrinted = false;
        particleSensor.clearFIFO(); // مسح القراءات القديمة في الـ FIFO
        delay(300);
    }

    // 5. معالجة القراءات بنظام النافذة المنزلقة (Sliding Window)
    bool readingValid = processSensorReadings();

    // 6. إرسال القراءة الناجحة والمطابقة للشروط الطبية إلى الداتابيز
    if (readingValid)
    {
        if (currentTime - lastSendTime >= SEND_INTERVAL)
        {
            lastSendTime = currentTime;
            sendDataToAPI();
        }
    }

    delay(100);
}
