using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAdlerCardBundleStudentByIdQuery : IRequest<Response<Domain.Entities.AdlerCardBundleStudent>>
    {
        public int Id { get; set; }
        public class GetAdlerCardBundleStudentByIdQueryHandler : IRequestHandler<GetAdlerCardBundleStudentByIdQuery, Response<Domain.Entities.AdlerCardBundleStudent>>
        {
            private readonly IAdlerCardBundleStudentRepositoryAsync _adlercardbundlestudentRepository;
            public GetAdlerCardBundleStudentByIdQueryHandler(IAdlerCardBundleStudentRepositoryAsync adlercardbundlestudentRepository)
            {
                _adlercardbundlestudentRepository = adlercardbundlestudentRepository;
            }
            public async Task<Response<Domain.Entities.AdlerCardBundleStudent>> Handle(GetAdlerCardBundleStudentByIdQuery query, CancellationToken cancellationToken)
            {
                var adlercardbundlestudent = await _adlercardbundlestudentRepository.GetByIdAsync(query.Id);
                if (adlercardbundlestudent == null) throw new ApiException($"AdlerCardBundleStudent Not Found.");
                return new Response<Domain.Entities.AdlerCardBundleStudent>(adlercardbundlestudent);
            }
        }
    }
}
