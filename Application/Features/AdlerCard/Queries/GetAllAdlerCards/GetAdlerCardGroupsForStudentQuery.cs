using Application.Filters;
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
    public class GetAdlerCardGroupsForStudentQuery : IRequest<IEnumerable<GetAdlerCardGroupsForStudentViewModel>>
    {
    }
    public class GetAdlerCardGroupsForStudentQueryHandler : IRequestHandler<GetAdlerCardGroupsForStudentQuery, IEnumerable<GetAdlerCardGroupsForStudentViewModel>>
    {
        private readonly IAdlerCardRepositoryAsync _adlercardRepository;
        private readonly IMapper _mapper;
        public GetAdlerCardGroupsForStudentQueryHandler(IAdlerCardRepositoryAsync adlercardRepository, IMapper mapper)
        {
            _adlercardRepository = adlercardRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAdlerCardGroupsForStudentViewModel>> Handle(GetAdlerCardGroupsForStudentQuery request, CancellationToken cancellationToken)
        {
            var adlercard = _adlercardRepository.GetAdlerCardGroupsForStudent();
            var adlercardViewModel = _mapper.Map<IEnumerable<GetAdlerCardGroupsForStudentViewModel>>(adlercard);
            return adlercardViewModel;
        }
    }
}
