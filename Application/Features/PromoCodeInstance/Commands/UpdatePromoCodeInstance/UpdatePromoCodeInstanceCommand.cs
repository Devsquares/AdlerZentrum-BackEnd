using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.PromoCodeInstance.Commands.UpdatePromoCodeInstance
{
	public class UpdatePromoCodeInstanceCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public int PromoCodeId { get; set; }
		//public PromoCode PromoCode { get; set; }
		public string PromoCodeKey { get; set; }
		public bool IsUsed { get; set; }
		public string StudentId { get; set; }
		//public ApplicationUser Student { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

        public class UpdatePromoCodeInstanceCommandHandler : IRequestHandler<UpdatePromoCodeInstanceCommand, Response<int>>
        {
            private readonly IPromoCodeInstanceRepositoryAsync _promocodeinstanceRepository;
            public UpdatePromoCodeInstanceCommandHandler(IPromoCodeInstanceRepositoryAsync promocodeinstanceRepository)
            {
                _promocodeinstanceRepository = promocodeinstanceRepository;
            }
            public async Task<Response<int>> Handle(UpdatePromoCodeInstanceCommand command, CancellationToken cancellationToken)
            {
                var promocodeinstance = await _promocodeinstanceRepository.GetByIdAsync(command.Id);

                if (promocodeinstance == null)
                {
                    throw new ApiException($"PromoCodeInstance Not Found.");
                }
                else
                {
				promocodeinstance.PromoCodeId = command.PromoCodeId;
				//promocodeinstance.PromoCode = command.PromoCode;
				promocodeinstance.PromoCodeKey = command.PromoCodeKey;
				promocodeinstance.IsUsed = command.IsUsed;
				promocodeinstance.StudentId = command.StudentId;
				//promocodeinstance.Student = command.Student;
				promocodeinstance.StartDate = command.StartDate;
				promocodeinstance.EndDate = command.EndDate; 

                    await _promocodeinstanceRepository.UpdateAsync(promocodeinstance);
                    return new Response<int>(promocodeinstance.Id);
                }
            }
        }

    }
}
