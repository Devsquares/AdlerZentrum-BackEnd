using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.AdlerCardBundleStudent.Commands.UpdateAdlerCardBundleStudent
{
	public class UpdateAdlerCardBundleStudentCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string StudentId { get; set; }
		//public ApplicationUser Student { get; set; }
		public int AdlerCardsBundleId { get; set; }
		//public AdlerCardsBundle AdlerCardsBundle { get; set; }
		public DateTime PurchasingDate { get; set; }

        public class UpdateAdlerCardBundleStudentCommandHandler : IRequestHandler<UpdateAdlerCardBundleStudentCommand, Response<int>>
        {
            private readonly IAdlerCardBundleStudentRepositoryAsync _adlercardbundlestudentRepository;
            public UpdateAdlerCardBundleStudentCommandHandler(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository)
            {
                _adlercardbundlestudentRepository = adlercardbundlestudentRepository;
            }
            public async Task<Response<int>> Handle(UpdateAdlerCardBundleStudentCommand command, CancellationToken cancellationToken)
            {
                var adlercardbundlestudent = await _adlercardbundlestudentRepository.GetByIdAsync(command.Id);

                if (adlercardbundlestudent == null)
                {
                    throw new ApiException($"AdlerCardBundleStudent Not Found.");
                }
                else
                {
				adlercardbundlestudent.StudentId = command.StudentId;
				//adlercardbundlestudent.Student = command.Student;
				adlercardbundlestudent.AdlerCardsBundleId = command.AdlerCardsBundleId;
				//adlercardbundlestudent.AdlerCardsBundle = command.AdlerCardsBundle;
				adlercardbundlestudent.PurchasingDate = command.PurchasingDate; 

                    await _adlercardbundlestudentRepository.UpdateAsync(adlercardbundlestudent);
                    return new Response<int>(adlercardbundlestudent.Id);
                }
            }
        }

    }
}
