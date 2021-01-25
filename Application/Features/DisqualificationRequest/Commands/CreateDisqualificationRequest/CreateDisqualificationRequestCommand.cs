using Application.Enums;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateDisqualificationRequestCommand : IRequest<Response<int>>
    {
		public string StudentId { get; set; } 
        public string Comment { get; set; }
    }

    public class CreateDisqualificationRequestCommandHandler : IRequestHandler<CreateDisqualificationRequestCommand, Response<int>>
    {
        private readonly IDisqualificationRequestRepositoryAsync _disqualificationrequestRepository;
        private readonly IMapper _mapper;
        public CreateDisqualificationRequestCommandHandler(IDisqualificationRequestRepositoryAsync disqualificationrequestRepository, IMapper mapper)
        {
            _disqualificationrequestRepository = disqualificationrequestRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateDisqualificationRequestCommand request, CancellationToken cancellationToken)
        {
            var disqualificationrequest = new DisqualificationRequest()
            {
                StudentId = request.StudentId,
                Comment = request.Comment
            }; // _mapper.Map<DisqualificationRequest>(request);

            disqualificationrequest.DisqualificationRequestStatus = (int)DisqualificationRequestStatusEnum.Pending;
            await _disqualificationrequestRepository.AddAsync(disqualificationrequest);
            return new Response<int>(disqualificationrequest.Id);
        }
    }
}
