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
    public partial class CreateAdlerCardSubmissionCommand : IRequest<Response<int>>
    {
		public AdlerCard AdlerCard { get; set; }
		public int AdlerCardId { get; set; }
		public ApplicationUser Student { get; set; }
		public string StudentId { get; set; }
		public ApplicationUser Teacher { get; set; }
		public int? TeacherId { get; set; }
		public int Status { get; set; }
		public double AchievedScore { get; set; }
    }

    public class CreateAdlerCardSubmissionCommandHandler : IRequestHandler<CreateAdlerCardSubmissionCommand, Response<int>>
    {
        private readonly IAdlerCardSubmissionRepositoryAsync _adlercardsubmissionRepository;
        private readonly IMapper _mapper;
        public CreateAdlerCardSubmissionCommandHandler(IAdlerCardSubmissionRepositoryAsync adlercardsubmissionRepository, IMapper mapper)
        {
            _adlercardsubmissionRepository = adlercardsubmissionRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateAdlerCardSubmissionCommand request, CancellationToken cancellationToken)
        {
            var adlercardsubmission = _mapper.Map<Domain.Entities.AdlerCardSubmission>(request);
            await _adlercardsubmissionRepository.AddAsync(adlercardsubmission);
            return new Response<int>(adlercardsubmission.Id);
        }
    }
}
