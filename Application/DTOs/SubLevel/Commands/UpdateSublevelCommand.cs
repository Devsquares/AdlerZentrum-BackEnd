using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateSublevelCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; } 
        public string Color { get; set; }
        public bool IsFinal { get; set; }

        public class UpdateSublevelCommandHandler : IRequestHandler<UpdateSublevelCommand, Response<int>>
        {
            private readonly ISublevelRepositoryAsync _SublevelRepository;
            public UpdateSublevelCommandHandler(ISublevelRepositoryAsync SublevelRepository)
            {
                _SublevelRepository = SublevelRepository;
            }
            public async Task<Response<int>> Handle(UpdateSublevelCommand command, CancellationToken cancellationToken)
            {
                var Sublevel = await _SublevelRepository.GetByIdAsync(command.Id);

                if (Sublevel == null)
                {
                    throw new ApiException($"Sublevel Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, Sublevel);
                    await _SublevelRepository.UpdateAsync(Sublevel);
                    return new Response<int>(Sublevel.Id);
                }
            }
        }
    }
}
