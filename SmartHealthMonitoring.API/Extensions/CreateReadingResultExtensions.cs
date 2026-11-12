using Microsoft.AspNetCore.Mvc;
using SmartHealthMonitoring.Domain.Enums;

namespace SmartHealthMonitoring.API.Extensions;

public static class CreateReadingResultExtensions
{
    public static IActionResult ToActionResult(
        this CreateReadingResult result,
        string sensorName,
        int? patientId,
        string successMessage)
    {
        return result switch
        {
            CreateReadingResult.NoPatientSelected =>
                new BadRequestObjectResult(new
                {
                    Message = "No patient is currently selected. Please select a patient before submitting measurements."
                }),
            CreateReadingResult.NotCurrentPatient =>
                new BadRequestObjectResult(new
                {
                    Message = $"{sensorName} PatientId {patientId} does not match the currently selected patient."
                }),
            CreateReadingResult.SessionMismatch =>
                new ConflictObjectResult(new
                {
                    Message = $"Active patient selection has changed. Please refresh current-patient before submitting {sensorName} measurements."
                }),
            CreateReadingResult.PatientNotFound =>
                new NotFoundObjectResult(new
                {
                    Message = patientId.HasValue
                        ? $"Patient with ID {patientId.Value} does not exist."
                        : "Selected patient does not exist."
                }),
            _ => new OkObjectResult(new
            {
                Message = successMessage
            })
        };
    }
}
