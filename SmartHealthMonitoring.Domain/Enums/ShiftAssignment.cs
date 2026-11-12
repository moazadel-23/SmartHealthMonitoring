using System.Text.Json.Serialization;

namespace SmartHealthMonitoring.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ShiftAssignment
{
    MorningShift = 0,
    AfternoonShift = 1,
    NightShift = 2
}
