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
    public partial class CreatePlacementUnReleaseCommand : IRequest<Response<bool>>
    {
        public int TestId { get; set; }
    }

    public class CreatePlacementUnReleaseCommandHandler : IRequestHandler<CreatePlacementUnReleaseCommand, Response<bool>>
    {
        private readonly IPlacementReleaseReopsitoryAsync _placementReleaseReopsitoryAsync;
        private readonly IMapper _mapper;
        public CreatePlacementUnReleaseCommandHandler(IMapper mapper, IPlacementReleaseReopsitoryAsync placementReleaseReopsitoryAsync)
        {
            _placementReleaseReopsitoryAsync = placementReleaseReopsitoryAsync;
        }

        public async Task<Response<bool>> Handle(CreatePlacementUnReleaseCommand request, CancellationToken cancellationToken)
        {
            var list = _placementReleaseReopsitoryAsync.GetByTest(request.TestId).Result;
            foreach (var item in list)
            {
                item.Cancel = true;
            }
            await _placementReleaseReopsitoryAsync.UpdateBulkAsync(list);
            return new Response<bool>(true);
        }
    }
}
