using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs.GroupInstance.Commands
{
    public class SwapTwoStudentsCommand : IRequest<Response<List<StudentsGroupInstanceModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public int SrcGroupInstanceId { get; set; }
        public string SrcStudentId { get; set; }
        public int DesGroupInstanceId { get; set; }
        public string DesStudentId { get; set; }
        public class SwapTwoStudentsCommandHandler : IRequestHandler<SwapTwoStudentsCommand, Response<List<StudentsGroupInstanceModel>>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public SwapTwoStudentsCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<List<StudentsGroupInstanceModel>>> Handle(SwapTwoStudentsCommand command, CancellationToken cancellationToken)
            {
                var swapStudents= _groupInstanceRepositoryAsync.SwapStudentBetweenTwoGroups(command.GroupDefinitionId, command.SrcGroupInstanceId, command.SrcStudentId, command.DesGroupInstanceId, command.DesStudentId);
                return new Response<List<StudentsGroupInstanceModel>>(swapStudents.Result);

            }
        }
    }
}
