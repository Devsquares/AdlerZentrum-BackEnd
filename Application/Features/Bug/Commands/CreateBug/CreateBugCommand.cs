using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features 
{
    public partial class CreateBugCommand : IRequest<Response<int>>
    {
		public string UserName { get; set; }
		public string BugName { get; set; }
		public string Type { get; set; }
		public string Priority { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
    }

    public class CreateBugCommandHandler : IRequestHandler<CreateBugCommand, Response<int>>
    {
        private readonly IBugRepositoryAsync _bugRepository;
        private readonly IMapper _mapper;
        public CreateBugCommandHandler(IBugRepositoryAsync bugRepository, IMapper mapper)
        {
            _bugRepository = bugRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateBugCommand request, CancellationToken cancellationToken)
        {
            var bug = _mapper.Map<Domain.Entities.Bug>(request);
            bug.Status = "Open";
            await _bugRepository.AddAsync(bug);
            return new Response<int>(bug.Id);
        }
    }
}
