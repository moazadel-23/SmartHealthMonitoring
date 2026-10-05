# 🏥 Smart Health Monitoring API

An enterprise-grade **.NET 9 Web API** built using **Clean Architecture** and **CQRS (MediatR)**. Designed to monitor real-time health vitals (Heart Rate, SpO2) received from **ESP32 / IoT sensors**, while managing doctors and patient records.

---

## 📐 Architecture Overview

The solution strictly adheres to **Clean Architecture** principles, maintaining a clear separation of concerns across 4 distinct layers:

```
SmartHealthMonitoring/
├── 🧪 SmartHealthMonitoring.Domain/          # Core entities & domain logic (Zero dependencies)
├── ⚙️ SmartHealthMonitoring.Application/     # CQRS Commands, Handlers, Interfaces & DTOs
├── 🗄️ SmartHealthMonitoring.Infrastructure/  # EF Core DbContext, Fluent Configurations & Repositories
└── 🚀 SmartHealthMonitoring.API/             # Controllers, Services registration & Scalar API Docs
```

---

## ✨ Features Implemented So Far

- **Doctor Management (`POST /api/Doctor`)**:
  - Register new medical doctors with contact info & specialization.
  - Data validation for email, phone, and name constraints.

- **Patient Management (`POST /api/Patient`)**:
  - Register new patients linked to a specific doctor (`DoctorId`).
  - Graceful validation: Checks if the target `DoctorId` exists before saving.

- **IoT Sensor Readings (`POST /api/Measurement`)**:
  - Ingest vital sensor readings (`HeartRate` and `SpO2`) from ESP32 hardware.
  - Data validation for valid Heart Rate (30–250 BPM) and SpO2 (70–100%) ranges.
  - Verification of patient existence prior to saving readings.

- **Modular Service Configuration**:
  - Extension methods inside `Services/ServiceRegistration.cs` keeping `Program.cs` clean and lightweight.

- **Interactive API Documentation**:
  - Integrated **Scalar API Reference** accessible at `/scalar/v1`.

---

## 🛠️ Technology Stack

- **Framework**: .NET 9 Web API
- **Architecture**: Clean Architecture & CQRS
- **Library for CQRS**: MediatR
- **ORM**: Entity Framework Core 9 (SQL Server)
- **API Docs**: Scalar.AspNetCore & OpenAPI

---

## 📡 API Endpoints Summary

### 1. Add Doctor
- **Route**: `POST /api/Doctor`
- **Request Body**:
```json
{
  "fName": "Ahmad",
  "lName": "Ali",
  "phone": "01012345678",
  "email": "ahmad.ali@example.com",
  "specialization": "Cardiology"
}
```
- **Response**: `200 OK`
```json
{
  "msg": "Doctor created successfully"
}
```

---

### 2. Add Patient
- **Route**: `POST /api/Patient`
- **Request Body**:
```json
{
  "fName": "Mohamed",
  "lName": "Hassan",
  "gender": "Male",
  "age": 45,
  "address": "Cairo, Egypt",
  "phone": "01123456789",
  "doctorId": 1
}
```
- **Response**: `200 OK`
```json
{
  "msg": "Patient created successfully"
}
```

---

### 3. Record Health Sensor Reading (ESP32 / IoT)
- **Route**: `POST /api/Measurement`
- **Request Body**:
```json
{
  "patientId": 1,
  "heartRate": 78.5,
  "spO2": 98.0
}
```
- **Response**: `200 OK`
```json
{
  "msg": "Measurement Created Successfully"
}
```

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server / LocalDB

### Database Setup
1. Configure your SQL Server connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=SmartHealthMonitoringDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

2. Apply migrations to update the database:
```bash
dotnet ef database update --project SmartHealthMonitoring.Infrastructure --startup-project SmartHealthMonitoring.API
```

### Running the API
```bash
dotnet run --project SmartHealthMonitoring.API
```
Access the interactive Scalar API documentation at: `http://localhost:<port>/scalar/v1`

---

## 📝 License
This project is open-source and maintained for the Smart Health Monitoring System.
