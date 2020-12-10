using Application.DTOs.HomeWorkSubmitionDTO.Queries;
using Application.Filters;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetAllHomeWorkSubmitionsQuery : IRequest<IEnumerable<GetAllHomeWorkSubmitionsViewModel>>
    {
        public string StudentId { get; set; }
        public int GroupInstanceId { get; set; }
    }
    public class GetAllHomeWorkSubmitionsQueryHandler : IRequestHandler<GetAllHomeWorkSubmitionsQuery, IEnumerable<GetAllHomeWorkSubmitionsViewModel>>
    {
        private readonly IHomeWorkSubmitionRepositoryAsync _HomeWorkSubmitionRepository;
        private readonly IMapper _mapper;
        public GetAllHomeWorkSubmitionsQueryHandler(IHomeWorkSubmitionRepositoryAsync HomeWorkSubmitionRepository, IMapper mapper)
        {
            _HomeWorkSubmitionRepository = HomeWorkSubmitionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllHomeWorkSubmitionsViewModel>> Handle(GetAllHomeWorkSubmitionsQuery request, CancellationToken cancellationToken)
        { 
            var HomeWorkSubmitions = await _HomeWorkSubmitionRepository.GetAllAsync(request.StudentId, request.GroupInstanceId);
            var userViewModel = _mapper.Map<IEnumerable<GetAllHomeWorkSubmitionsViewModel>>(HomeWorkSubmitions);
            return userViewModel;
        }
    }
}
