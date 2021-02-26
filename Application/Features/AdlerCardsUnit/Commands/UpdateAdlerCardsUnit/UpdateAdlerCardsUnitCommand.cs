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
    public class UpdateAdlerCardsUnitCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Level Level { get; set; }
        public int LevelId { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public int Order { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        public class UpdateAdlerCardsUnitCommandHandler : IRequestHandler<UpdateAdlerCardsUnitCommand, Response<int>>
        {
            private readonly IAdlerCardsUnitRepositoryAsync _adlercardsunitRepository;
            public UpdateAdlerCardsUnitCommandHandler(IAdlerCardsUnitRepositoryAsync adlercardsunitRepository)
            {
                _adlercardsunitRepository = adlercardsunitRepository;
            }
            public async Task<Response<int>> Handle(UpdateAdlerCardsUnitCommand command, CancellationToken cancellationToken)
            {
                var adlercardsunit = await _adlercardsunitRepository.GetByIdAsync(command.Id);

                if (adlercardsunit == null)
                {
                    throw new ApiException($"AdlerCardsUnit Not Found.");
                }
                else
                {
                    adlercardsunit.Name = command.Name;
                    adlercardsunit.Level = command.Level;
                    adlercardsunit.LevelId = command.LevelId;
                    adlercardsunit.AdlerCardsTypeId = command.AdlerCardsTypeId;
                    adlercardsunit.Order = command.Order;
                    adlercardsunit.Image = command.Image;
                    adlercardsunit.Description = command.Description;

                    await _adlercardsunitRepository.UpdateAsync(adlercardsunit);
                    return new Response<int>(adlercardsunit.Id);
                }
            }
        }

    }
}
