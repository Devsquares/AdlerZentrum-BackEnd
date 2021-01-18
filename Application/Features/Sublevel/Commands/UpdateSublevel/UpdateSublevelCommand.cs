using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class UpdateSublevelCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LevelId { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }

        public class UpdateSublevelCommandHandler : IRequestHandler<UpdateSublevelCommand, Response<int>>
        {
            private readonly ISublevelRepositoryAsync _sublevelRepository;
            public UpdateSublevelCommandHandler(ISublevelRepositoryAsync sublevelRepository)
            {
                _sublevelRepository = sublevelRepository;
            }
            public async Task<Response<int>> Handle(UpdateSublevelCommand command, CancellationToken cancellationToken)
            {
                var sublevel = await _sublevelRepository.GetByIdAsync(command.Id);

                if (sublevel == null)
                {
                    throw new ApiException($"Sublevel Not Found.");
                }
                else
                {
                    sublevel.Name = command.Name;
                    sublevel.LevelId = command.LevelId;
                    sublevel.NumberOflessons = command.NumberOflessons;
                    sublevel.Color = command.Color;

                    await _sublevelRepository.UpdateAsync(sublevel);
                    return new Response<int>(sublevel.Id);
                }
            }
        }

    }
}
