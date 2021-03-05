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

namespace Application.Features.ForumReply.Commands.CreateForumReply
{
    public partial class CreateForumReplyCommand : IRequest<Response<int>>
    {
        public string WriterId { get; set; }
        public string Text { get; set; }
		public byte[] Image { get; set; }
		public int ForumCommentId { get; set; }
    }

    public class CreateForumReplyCommandHandler : IRequestHandler<CreateForumReplyCommand, Response<int>>
    {
        private readonly IForumReplyRepositoryAsync _forumreplyRepository;
        private readonly IMapper _mapper;
        public CreateForumReplyCommandHandler(IForumReplyRepositoryAsync forumreplyRepository, IMapper mapper)
        {
            _forumreplyRepository = forumreplyRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateForumReplyCommand request, CancellationToken cancellationToken)
        {
            var forumreply = _mapper.Map<Domain.Entities.ForumReply>(request);
            await _forumreplyRepository.AddAsync(forumreply);
            return new Response<int>(forumreply.Id);
        }
    }
}
