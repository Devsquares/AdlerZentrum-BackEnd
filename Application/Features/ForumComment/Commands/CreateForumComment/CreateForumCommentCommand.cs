using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ForumComment.Commands.CreateForumComment
{
    public partial class CreateForumCommentCommand : IRequest<Response<int>>
    {
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public int ForumTopicId { get; set; }
		public string WriterId { get; set; }
    }

    public class CreateForumCommentCommandHandler : IRequestHandler<CreateForumCommentCommand, Response<int>>
    {
        private readonly IForumCommentRepositoryAsync _forumcommentRepository;
        private readonly IMapper _mapper;
        public CreateForumCommentCommandHandler(IForumCommentRepositoryAsync forumcommentRepository, IMapper mapper)
        {
            _forumcommentRepository = forumcommentRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateForumCommentCommand request, CancellationToken cancellationToken)
        {
            var forumcomment = _mapper.Map<Domain.Entities.ForumComment>(request);
            await _forumcommentRepository.AddAsync(forumcomment);
            return new Response<int>(forumcomment.Id);
        }
    }
}
