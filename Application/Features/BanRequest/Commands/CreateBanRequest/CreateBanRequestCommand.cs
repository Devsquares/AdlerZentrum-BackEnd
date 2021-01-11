using Application.Enums;
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
    public partial class CreateBanRequestCommand : IRequest<Response<int>>
    {
        public string StudentId { get; set; }
        public string Comment { get; set; }
    }

    public class CreateBanRequestCommandHandler : IRequestHandler<CreateBanRequestCommand, Response<int>>
    {
        private readonly IBanRequestRepositoryAsync _banrequestRepository;
        private readonly IMapper _mapper;
        public CreateBanRequestCommandHandler(IBanRequestRepositoryAsync banrequestRepository, IMapper mapper)
        {
            _banrequestRepository = banrequestRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateBanRequestCommand request, CancellationToken cancellationToken)
        {
            var banrequest = _mapper.Map<BanRequest>(request);
            banrequest.BanRequestStatus = (int)BanRequestStatusEnum.New;
            await _banrequestRepository.AddAsync(banrequest);
            return new Response<int>(banrequest.Id);
        }
    }
}
