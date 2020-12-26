using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.TestInstance.Commands.UpdateTestInstance
{
	public class UpdateTestInstanceCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public int LessonInstanceId { get; set; }
		public string StudentId { get; set; }
		public int Points { get; set; }
		public int Status { get; set; }
		public LessonInstance LessonInstance { get; set; }
		public DateTime StartDate { get; set; }

        public class UpdateTestInstanceCommandHandler : IRequestHandler<UpdateTestInstanceCommand, Response<int>>
        {
            private readonly ITestInstanceRepositoryAsync _testinstanceRepository;
            public UpdateTestInstanceCommandHandler(ITestInstanceRepositoryAsync testinstanceRepository)
            {
                _testinstanceRepository = testinstanceRepository;
            }
            public async Task<Response<int>> Handle(UpdateTestInstanceCommand command, CancellationToken cancellationToken)
            {
                var testinstance = await _testinstanceRepository.GetByIdAsync(command.Id);

                if (testinstance == null)
                {
                    throw new ApiException($"TestInstance Not Found.");
                }
                else
                {
				testinstance.LessonInstanceId = command.LessonInstanceId;
				testinstance.StudentId = command.StudentId;
				testinstance.Points = command.Points;
				testinstance.Status = command.Status;
				testinstance.LessonInstance = command.LessonInstance;
				testinstance.StartDate = command.StartDate; 

                    await _testinstanceRepository.UpdateAsync(testinstance);
                    return new Response<int>(testinstance.Id);
                }
            }
        }

    }
}
