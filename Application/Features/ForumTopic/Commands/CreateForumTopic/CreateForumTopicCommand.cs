using Application.Enums;
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

namespace Application.Features.ForumTopic.Commands.CreateForumTopic
{
    public partial class CreateForumTopicCommand : IRequest<Response<int>>
    {
        public string WriterId { get; set; }
        public string Header { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public Enums.ForumType ForumType { get; set; }
		public int GroupInstanceId { get; set; }
		public int GroupDefinitionId { get; set; }
    }

    public class CreateForumTopicCommandHandler : IRequestHandler<CreateForumTopicCommand, Response<int>>
    {
        private readonly IForumTopicRepositoryAsync _forumtopicRepository;
        private readonly IMapper _mapper;
        public CreateForumTopicCommandHandler(IForumTopicRepositoryAsync forumtopicRepository, IMapper mapper)
        {
            _forumtopicRepository = forumtopicRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateForumTopicCommand request, CancellationToken cancellationToken)
        {
            var forumtopic = _mapper.Map<Domain.Entities.ForumTopic>(request);
            await _forumtopicRepository.AddAsync(forumtopic);
            return new Response<int>(forumtopic.Id);
        }
    }
}
