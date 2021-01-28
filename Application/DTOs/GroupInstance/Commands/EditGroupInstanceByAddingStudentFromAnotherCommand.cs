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
    public class EditGroupInstanceByAddingStudentFromAnotherCommand : IRequest<Response<List<StudentsGroupInstanceModel>>>
    {
        public int GroupDefinitionId { get; set; }
        public int srcGroupInstanceId { get; set; }
        public int desGroupInstanceId { get; set; }
        public string studentId { get; set; }
        public int? promoCodeId { get; set; }
        public class EditGroupInstanceByAddingStudentFromAnotherCommandHandler : IRequestHandler<EditGroupInstanceByAddingStudentFromAnotherCommand, Response<List<StudentsGroupInstanceModel>>>
        {
            private readonly IGroupInstanceRepositoryAsync _groupInstanceRepositoryAsync;
            public EditGroupInstanceByAddingStudentFromAnotherCommandHandler(IGroupInstanceRepositoryAsync groupInstanceRepository)
            {
                _groupInstanceRepositoryAsync = groupInstanceRepository;
            }
            public async Task<Response<List<StudentsGroupInstanceModel>>> Handle(EditGroupInstanceByAddingStudentFromAnotherCommand command, CancellationToken cancellationToken)
            {
                var editGroupInstance = _groupInstanceRepositoryAsync.EditGroupByAddStudentFromAnotherGroup(command.GroupDefinitionId, command.srcGroupInstanceId, command.desGroupInstanceId, command.studentId, command.promoCodeId);
                return new Response<List<StudentsGroupInstanceModel>>(editGroupInstance.Result);

            }
        }
    }
}
