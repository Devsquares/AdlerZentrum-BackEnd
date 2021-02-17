using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.InterestedStudent.Commands.UpdateInterestedStudent
{
	public class UpdateInterestedStudentCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public int PromoCodeInstanceId { get; set; }
		public string StudentId { get; set; }
		public int GroupDefinitionId { get; set; }

        public class UpdateInterestedStudentCommandHandler : IRequestHandler<UpdateInterestedStudentCommand, Response<int>>
        {
            private readonly IInterestedStudentRepositoryAsync _interestedstudentRepository;
            public UpdateInterestedStudentCommandHandler(IInterestedStudentRepositoryAsync interestedstudentRepository)
            {
                _interestedstudentRepository = interestedstudentRepository;
            }
            public async Task<Response<int>> Handle(UpdateInterestedStudentCommand command, CancellationToken cancellationToken)
            {
                var interestedstudent = await _interestedstudentRepository.GetByIdAsync(command.Id);

                if (interestedstudent == null)
                {
                    throw new ApiException($"InterestedStudent Not Found.");
                }
                else
                {
				interestedstudent.PromoCodeInstanceId = command.PromoCodeInstanceId;
				interestedstudent.StudentId = command.StudentId;
				interestedstudent.GroupDefinitionId = command.GroupDefinitionId;

                    await _interestedstudentRepository.UpdateAsync(interestedstudent);
                    return new Response<int>(interestedstudent.Id);
                }
            }
        }

    }
}
