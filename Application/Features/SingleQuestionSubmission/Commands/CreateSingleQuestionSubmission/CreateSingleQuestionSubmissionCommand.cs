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
    public partial class CreateSingleQuestionSubmissionCommand : IRequest<Response<int>>
    {
		public string AnswerText { get; set; }
		public bool TrueOrFalseSubmission { get; set; }
		public string StudentId { get; set; }
		public ICollection<ChoiceSubmission> Choices { get; set; }
		public ApplicationUser Student { get; set; }
		public SingleQuestion SingleQuestion { get; set; }
    }

    public class CreateSingleQuestionSubmissionCommandHandler : IRequestHandler<CreateSingleQuestionSubmissionCommand, Response<int>>
    {
        private readonly ISingleQuestionSubmissionRepositoryAsync _singlequestionsubmissionRepository;
        private readonly IMapper _mapper;
        public CreateSingleQuestionSubmissionCommandHandler(ISingleQuestionSubmissionRepositoryAsync singlequestionsubmissionRepository, IMapper mapper)
        {
            _singlequestionsubmissionRepository = singlequestionsubmissionRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateSingleQuestionSubmissionCommand request, CancellationToken cancellationToken)
        {
            var singlequestionsubmission = _mapper.Map<Domain.Entities.SingleQuestionSubmission>(request);
            await _singlequestionsubmissionRepository.AddAsync(singlequestionsubmission);
            return new Response<int>(singlequestionsubmission.Id);
        }
    }
}
