using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
	public class UpdateOverPaymentStudentCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string StudentId { get; set; }
		public int GroupDefinitionId { get; set; }

        public class UpdateOverPaymentStudentCommandHandler : IRequestHandler<UpdateOverPaymentStudentCommand, Response<int>>
        {
            private readonly IOverPaymentStudentRepositoryAsync _overpaymentstudentRepository;
            public UpdateOverPaymentStudentCommandHandler(IOverPaymentStudentRepositoryAsync overpaymentstudentRepository)
            {
                _overpaymentstudentRepository = overpaymentstudentRepository;
            }
            public async Task<Response<int>> Handle(UpdateOverPaymentStudentCommand command, CancellationToken cancellationToken)
            {
                var overpaymentstudent = await _overpaymentstudentRepository.GetByIdAsync(command.Id);

                if (overpaymentstudent == null)
                {
                    throw new ApiException($"OverPaymentStudent Not Found.");
                }
                else
                {
				overpaymentstudent.StudentId = command.StudentId;
				overpaymentstudent.GroupDefinitionId = command.GroupDefinitionId;

                    await _overpaymentstudentRepository.UpdateAsync(overpaymentstudent);
                    return new Response<int>(overpaymentstudent.Id);
                }
            }
        }

    }
}
