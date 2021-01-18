using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateSublevelCommand : IRequest<Response<int>>
    {
		public string Name { get; set; }
		public int LevelId { get; set; } 
		public int NumberOflessons { get; set; }
		public string Color { get; set; } 
    }

    public class CreateSublevelCommandHandler : IRequestHandler<CreateSublevelCommand, Response<int>>
    {
        private readonly ISublevelRepositoryAsync _sublevelRepository;
        private readonly IMapper _mapper;
        public CreateSublevelCommandHandler(ISublevelRepositoryAsync sublevelRepository, IMapper mapper)
        {
            _sublevelRepository = sublevelRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateSublevelCommand request, CancellationToken cancellationToken)
        {
            var sublevel = _mapper.Map<Domain.Entities.Sublevel>(request);
            await _sublevelRepository.AddAsync(sublevel);
            return new Response<int>(sublevel.Id);
        }
    }
}
