using MediatR;
using SmartHealthMonitoring.Application.Interfaces;
using SmartHealthMonitoring.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHealthMonitoring.Application.Features.Reception.Nurses.Commands.CreateNurse
{
    public class CreateNurseHandller : IRequestHandler<CreateNurseCommands, bool>
    {
        private readonly INurseRepository _repository;

        public CreateNurseHandller(INurseRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(CreateNurseCommands request, CancellationToken cancellationToken)
        {
            var nurse = new Nurse
            {
                FName = request.FName,
                LName = request.LName,
                Phone = request.Phone,
                Email = request.Email,
                Specialization = request.Specialization,
                Assignment = request.Assignment
            };
            await _repository.AddAsync(nurse, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
