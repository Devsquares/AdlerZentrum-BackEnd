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
    public class UpdateLevelCommand : IRequest<Response<int>>
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Sublevel> SubLevels { get; set; }

        public class UpdateLevelCommandHandler : IRequestHandler<UpdateLevelCommand, Response<int>>
        {
            private readonly ILevelRepositoryAsync _levelRepository;
            public UpdateLevelCommandHandler(ILevelRepositoryAsync levelRepository)
            {
                _levelRepository = levelRepository;
            }
            public async Task<Response<int>> Handle(UpdateLevelCommand command, CancellationToken cancellationToken)
            {
                var level = await _levelRepository.GetByIdAsync(command.Id);

                if (level == null)
                {
                    throw new ApiException($"Level Not Found.");
                }
                else
                {
                    Reflection.CopyProperties(command, level);
                    await _levelRepository.UpdateAsync(level);
                    return new Response<int>(level.Id);
                }
            }
        }
    }
}
