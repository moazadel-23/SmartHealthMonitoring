using MediatR;
using SmartHealthMonitoring.Application.DTOs;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Queries.GetDoctor;

public record GetDoctorQuery(int Id) : IRequest<DoctorResponseDto?>;
