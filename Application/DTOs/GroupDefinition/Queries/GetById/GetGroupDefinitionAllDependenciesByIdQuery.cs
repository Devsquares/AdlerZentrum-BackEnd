using Application.DTOs.GroupDefinition.Queries.GetById;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetGroupDefinitionAllDependenciesByIdQuery : IRequest<Response<GroupDefinitionDependenciesModel>>
    {
        public int GroupDefinitionId { get; set; }
        public class GetGroupDefinitionAllDependenciesByIdQueryHandler : IRequestHandler<GetGroupDefinitionAllDependenciesByIdQuery, Response<GroupDefinitionDependenciesModel>>
        {
            private readonly IGroupDefinitionRepositoryAsync _GroupDefinitionRepository;
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            private readonly IOverPaymentStudentRepositoryAsync _overPaymentStudentRepositoryAsync;
            private readonly IInterestedStudentRepositoryAsync _interestedStudentRepositoryAsync;
            public GetGroupDefinitionAllDependenciesByIdQueryHandler(IGroupInstanceRepositoryAsync groupInstanceRepositoryAsync, 
                IGroupDefinitionRepositoryAsync GroupDefinitionRepository,
                IOverPaymentStudentRepositoryAsync overPaymentStudentRepositoryAsync,
                IInterestedStudentRepositoryAsync interestedStudentRepositoryAsync)
            {
                _GroupDefinitionRepository = GroupDefinitionRepository;
                _groupInstanceRepositoryAsync = groupInstanceRepositoryAsync;
                _overPaymentStudentRepositoryAsync = overPaymentStudentRepositoryAsync;
                _interestedStudentRepositoryAsync = interestedStudentRepositoryAsync;
            }
            public async Task<Response<GroupDefinitionDependenciesModel>> Handle(GetGroupDefinitionAllDependenciesByIdQuery query, CancellationToken cancellationToken)
            {
                var GroupDefinition = _GroupDefinitionRepository.GetById(query.GroupDefinitionId);
                if (GroupDefinition == null) throw new ApiException($"Group Not Found.");
                var allgroupStudents = _groupInstanceRepositoryAsync.GetListByGroupDefinitionId(query.GroupDefinitionId);
                var interested = _interestedStudentRepositoryAsync.GetByGroupDefinitionId(query.GroupDefinitionId);
                var overPayment = _overPaymentStudentRepositoryAsync.GetByGroupDefinitionId(query.GroupDefinitionId);
                GroupDefinitionDependenciesModel groupDefinitionDependenciesModel = new GroupDefinitionDependenciesModel();
                groupDefinitionDependenciesModel.GroupDefinition = GroupDefinition;
                groupDefinitionDependenciesModel.GroupInstancesStudents = allgroupStudents;
                groupDefinitionDependenciesModel.InterestedStudents = interested;
                groupDefinitionDependenciesModel.OverPaymentStudents = overPayment;
                return new Response<GroupDefinitionDependenciesModel>(groupDefinitionDependenciesModel);
            }
        }
    }
}
