using MediatR;
using SmartHealthMonitoring.Application.DTOs;
using System.Collections.Generic;

namespace SmartHealthMonitoring.Application.Features.Reception.Doctors.Queries.GetAllDoctors;

public record GetAllDoctorsQuery() : IRequest<IEnumerable<DoctorResponseDto>>;
