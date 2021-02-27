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

namespace Application.Features
{
	public class UpdateAdlerCardSubmissionCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public AdlerCard AdlerCard { get; set; }
		public int AdlerCardId { get; set; }
		public ApplicationUser Student { get; set; }
		public string StudentId { get; set; }
		public ApplicationUser Teacher { get; set; }
		public int? TeacherId { get; set; }
		public int Status { get; set; }
		public double AchievedScore { get; set; }

        public class UpdateAdlerCardSubmissionCommandHandler : IRequestHandler<UpdateAdlerCardSubmissionCommand, Response<int>>
        {
            private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
            public UpdateAdlerCardSubmissionCommandHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository)
            {
                _adlercardsubmissionRepository = adlercardsubmissionRepository;
            }
            public async Task<Response<int>> Handle(UpdateAdlerCardSubmissionCommand command, CancellationToken cancellationToken)
            {
                var adlercardsubmission = await _adlercardsubmissionRepository.GetByIdAsync(command.Id);

                if (adlercardsubmission == null)
                {
                    throw new ApiException($"AdlerCardSubmission Not Found.");
                }
                else
                {
				adlercardsubmission.AdlerCard = command.AdlerCard;
				adlercardsubmission.AdlerCardId = command.AdlerCardId;
				adlercardsubmission.Student = command.Student;
				adlercardsubmission.StudentId = command.StudentId;
				adlercardsubmission.Teacher = command.Teacher;
				adlercardsubmission.TeacherId = command.TeacherId;
				adlercardsubmission.Status = command.Status;
				adlercardsubmission.AchievedScore = command.AchievedScore; 

                    await _adlercardsubmissionRepository.UpdateAsync(adlercardsubmission);
                    return new Response<int>(adlercardsubmission.Id);
                }
            }
        }

    }
}
