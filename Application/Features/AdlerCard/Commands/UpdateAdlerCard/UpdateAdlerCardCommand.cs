using Application.Enums;
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
        public Domain.Entities.AdlerCardsUnit AdlerCardsUnit { get; set; }
        public int AdlerCardsUnitId { get; set; }
        public Question Question { get; set; }
        public int AllowedDuration { get; set; }
        public double TotalScore { get; set; }
        public int Status { get; set; }
        public int AdlerCardsTypeId { get; set; }

        public class UpdateAdlerCardCommandHandler : IRequestHandler<UpdateAdlerCardCommand, Response<int>>
        {
            private readonly IAdlerCardRepositoryAsync _adlercardRepository;
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardUnitRepository;
            private readonly IQuestionRepositoryAsync _questionRepository;
            public UpdateAdlerCardCommandHandler(IAdlerCardRepositoryAsync adlercardRepository,
               IAdlerCardsUnitRepositoryAsync adlercardUnitRepository,
               IQuestionRepositoryAsync questionRepositoryAsync)
            {
                _adlercardRepository = adlercardRepository;
                _adlercardUnitRepository = adlercardUnitRepository;
                _questionRepository = questionRepositoryAsync;
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
                    if (adlercard.Status != (int)AdlerCardEnum.Draft) throw new ApiException("Cann't edit adler card.");
                    var adlerCardUnit = _adlercardUnitRepository.GetByIdAsync(command.AdlerCardsUnitId).Result;
                    if (adlerCardUnit == null)
                    {
                        throw new ApiException("No Adler Card Unite");
                    }
                    if (command.AdlerCardsTypeId != adlerCardUnit.AdlerCardsTypeId)
                    {
                        throw new ApiException("The Type of Adler Card isn't the same as Adler Card unit");
                    }
                    await _questionRepository.DeleteAsync(adlercard.Question);
                    var question = await _questionRepository.AddAsync(command.Question);
                    adlercard.Name = command.Name;
                    adlercard.AdlerCardsUnit = command.AdlerCardsUnit;
                    adlercard.AdlerCardsUnitId = command.AdlerCardsUnitId;
                    adlercard.Question = question;
                    adlercard.QuestionId = question.Id;
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
