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
    public partial class CreateChoiceSubmissionCommand : IRequest<Response<int>>
    {
		public Choice Choice { get; set; }
		public int ChoiceId { get; set; }
		public SingleQuestionSubmission SingleQuestionSubmission { get; set; }
		public int SingleQuestionSubmissionId { get; set; }
    }

    public class CreateChoiceSubmissionCommandHandler : IRequestHandler<CreateChoiceSubmissionCommand, Response<int>>
    {
        private readonly IChoiceSubmissionRepositoryAsync _choicesubmissionRepository;
        private readonly IMapper _mapper;
        public CreateChoiceSubmissionCommandHandler(IChoiceSubmissionRepositoryAsync choicesubmissionRepository, IMapper mapper)
        {
            _choicesubmissionRepository = choicesubmissionRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateChoiceSubmissionCommand request, CancellationToken cancellationToken)
        {
            var choicesubmission = _mapper.Map<Domain.Entities.ChoiceSubmission>(request);
            await _choicesubmissionRepository.AddAsync(choicesubmission);
            return new Response<int>(choicesubmission.Id);
        }
    }
}
