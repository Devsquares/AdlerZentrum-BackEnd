using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using Domain.Utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features
{
    public partial class CreatePlacementReleaseCommand : IRequest<Response<int>>
    {
        public int TestId { get; set; }
        public DateTime RelaeseDate { get; set; }
    }

    public class CreatePlacementReleaseCommandHandler : IRequestHandler<CreatePlacementReleaseCommand, Response<int>>
    {
        private readonly IPlacementReleaseReopsitoryAsync _placementReleaseReopsitoryAsync;
        private readonly IMapper _mapper;
        public CreatePlacementReleaseCommandHandler(IMapper mapper, IPlacementReleaseReopsitoryAsync placementReleaseReopsitoryAsync)
        {
            _placementReleaseReopsitoryAsync = placementReleaseReopsitoryAsync;
        }

        public async Task<Response<int>> Handle(CreatePlacementReleaseCommand request, CancellationToken cancellationToken)
        {
            PlacementRelease placementRelease = new PlacementRelease();

            Reflection.CopyProperties(request, placementRelease);
            await _placementReleaseReopsitoryAsync.AddAsync(placementRelease);
            return new Response<int>(placementRelease.Id);
        }
    }
}
