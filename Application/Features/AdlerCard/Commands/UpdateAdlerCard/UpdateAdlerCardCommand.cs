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
	public class UpdateAdlerCardCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public AdlerCardsUnit AdlerCardsUnit { get; set; }
		public int AdlerCardsUnitId { get; set; }
		public Question Question { get; set; }
		public int QuestionId { get; set; }
		public int AllowedDuration { get; set; }
		public double TotalScore { get; set; }
		public int Status { get; set; }
		public int AdlerCardsTypeId { get; set; }

        public class UpdateAdlerCardCommandHandler : IRequestHandler<UpdateAdlerCardCommand, Response<int>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardUnitRepository;
            public UpdateAdlerCardCommandHandler(IAdlerCardRepositoryAsync adlercardRepository, IAdlerCardsUnitRepositoryAsync adlercardUnitRepository)
            {
                _adlercardRepository = adlercardRepository;
                _adlercardUnitRepository = adlercardUnitRepository;
            }
            public async Task<Response<int>> Handle(UpdateAdlerCardCommand command, CancellationToken cancellationToken)
            {
                var adlercard = await _adlercardRepository.GetByIdAsync(command.Id);

                if (adlercard == null)
                {
                    throw new ApiException($"AdlerCard Not Found.");
                }
                else
                {
                    var adlerCardUnit = _adlercardUnitRepository.GetByIdAsync(command.AdlerCardsUnitId).Result;
                    if (adlerCardUnit == null)
                    {
                        throw new ApiException("No Adler Card Unite");
                    }
                    if (command.AdlerCardsTypeId != adlerCardUnit.AdlerCardsTypeId)
                    {
                        throw new ApiException("The Type of Adler Card isn't the same as Adler Card unit");
                    }
                    adlercard.Name = command.Name;
				adlercard.AdlerCardsUnit = command.AdlerCardsUnit;
				adlercard.AdlerCardsUnitId = command.AdlerCardsUnitId;
				adlercard.Question = command.Question;
				adlercard.QuestionId = command.QuestionId;
				adlercard.AllowedDuration = command.AllowedDuration;
				adlercard.TotalScore = command.TotalScore;
				adlercard.Status = command.Status;
				adlercard.AdlerCardsTypeId = command.AdlerCardsTypeId; 

                    await _adlercardRepository.UpdateAsync(adlercard);
                    return new Response<int>(adlercard.Id);
                }
            }
        }

    }
}
