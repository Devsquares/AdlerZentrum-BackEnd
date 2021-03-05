using Application.Enums;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public partial class CreateAdlerCardCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public int AdlerCardsUnitId { get; set; }
        public int QuestionId { get; set; }
        public int AllowedDuration { get; set; }
        public double TotalScore { get; set; }
       // public int Status { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public int LevelId { get; set; }
    }

    public class CreateAdlerCardCommandHandler : IRequestHandler<CreateAdlerCardCommand, Response<int>>
    {
        private readonly IAdlerCardRepositoryAsync _adlercardRepository;
        private readonly IAdlerCardsUnitRepositoryAsync _adlercardUnitRepository;
        private readonly IMapper _mapper;
        public CreateAdlerCardCommandHandler(IAdlerCardRepositoryAsync adlercardRepository, IMapper mapper, IAdlerCardsUnitRepositoryAsync adlercardUnitRepository)
        {
            _adlercardRepository = adlercardRepository;
            _mapper = mapper;
            _adlercardUnitRepository = adlercardUnitRepository;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardCommand request, CancellationToken cancellationToken)
        {
            var adlerCardUnit = _adlercardUnitRepository.GetByIdAsync(request.AdlerCardsUnitId).Result;
            if(adlerCardUnit == null)
            {
                throw new ApiException("No Adler Card Unite");
            }
            if(request.AdlerCardsTypeId != adlerCardUnit.AdlerCardsTypeId)
            {
                throw new ApiException("The Type of Adler Card isn't the same as Adler Card unit");
            }
            var adlercard = _mapper.Map<Domain.Entities.AdlerCard>(request);
            adlercard.Status = (int)AdlerCardEnum.Draft;
            await _adlercardRepository.AddAsync(adlercard);
            return new Response<int>(adlercard.Id);
        }
    }
}
