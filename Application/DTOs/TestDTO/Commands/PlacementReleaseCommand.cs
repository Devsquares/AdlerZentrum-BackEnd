using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PlacementReleaseCommand : IRequest<Response<int>>
    {
        public int PlacementTestId { get; set; }
        public DateTime? StartDate { get; set; }
        public class PlacementReleaseCommandHandler : IRequestHandler<PlacementReleaseCommand, Response<int>>
        {
            private readonly ITestRepositoryAsync _TestRepositoryAsync;
            private readonly IMediator _mediator;
            public PlacementReleaseCommandHandler(ITestRepositoryAsync TestRepositoryAsync, IMediator mediator)
            {
                _TestRepositoryAsync = TestRepositoryAsync;
                _mediator = mediator;
            }
            public async Task<Response<int>> Handle(PlacementReleaseCommand command, CancellationToken cancellationToken)
            {
                var Test = await _TestRepositoryAsync.GetByIdAsync(command.PlacementTestId);
                if (Test == null) throw new ApiException($"Test Not Found.");
                Test.Status = (int)TestStatusEnum.Final;
                Test.PlacementStartDate = command.StartDate;

                await _TestRepositoryAsync.UpdateAsync(Test);
                return new Response<int>(Test.Id);
            }
        }
    }
}
