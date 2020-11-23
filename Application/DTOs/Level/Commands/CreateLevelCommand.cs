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

namespace Application.DTOs.Level.Commands
{
    public class CreateLevelCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Sublevel> SubLevels { get; set; }

        public class CreateLevelCommandHandler : IRequestHandler<CreateLevelCommand, Response<int>>
        {
            private readonly ILevelRepositoryAsync _levelRepository;
            public CreateLevelCommandHandler(ILevelRepositoryAsync levelRepository)
            {
                _levelRepository = levelRepository;
            }
            public async Task<Response<int>> Handle(CreateLevelCommand command, CancellationToken cancellationToken)
            {
                var level = new Domain.Entities.Level();

                Reflection.CopyProperties(command, level);
                await _levelRepository.AddAsync(level);
                return new Response<int>(level.Id);

            }
        }
    }
}
