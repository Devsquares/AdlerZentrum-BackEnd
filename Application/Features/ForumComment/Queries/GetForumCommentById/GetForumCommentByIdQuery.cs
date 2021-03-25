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
    public class GetForumCommentByIdQuery : IRequest<Response<Domain.Entities.ForumComment>>
    {
        public int Id { get; set; }
        public class GetForumCommentByIdQueryHandler : IRequestHandler<GetForumCommentByIdQuery, Response<Domain.Entities.ForumComment>>
        {
            private readonly IForumCommentRepositoryAsync _forumcommentRepository;
            public GetForumCommentByIdQueryHandler(IForumCommentRepositoryAsync forumcommentRepository)
            {
                _forumcommentRepository = forumcommentRepository;
            }
            public async Task<Response<Domain.Entities.ForumComment>> Handle(GetForumCommentByIdQuery query, CancellationToken cancellationToken)
            {
                var forumcomment = await _forumcommentRepository.GetByIdAsync(query.Id);
                if (forumcomment == null) throw new ApiException($"ForumComment Not Found.");
                return new Response<Domain.Entities.ForumComment>(forumcomment);
            }
        }
    }
}
