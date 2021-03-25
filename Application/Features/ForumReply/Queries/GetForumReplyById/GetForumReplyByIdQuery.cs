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
    public class GetForumReplyByIdQuery : IRequest<Response<Domain.Entities.ForumReply>>
    {
        public int Id { get; set; }
        public class GetForumReplyByIdQueryHandler : IRequestHandler<GetForumReplyByIdQuery, Response<Domain.Entities.ForumReply>>
        {
            private readonly IForumReplyRepositoryAsync _forumreplyRepository;
            public GetForumReplyByIdQueryHandler(IForumReplyRepositoryAsync forumreplyRepository)
            {
                _forumreplyRepository = forumreplyRepository;
            }
            public async Task<Response<Domain.Entities.ForumReply>> Handle(GetForumReplyByIdQuery query, CancellationToken cancellationToken)
            {
                var forumreply = await _forumreplyRepository.GetByIdAsync(query.Id);
                if (forumreply == null) throw new ApiException($"ForumReply Not Found.");
                return new Response<Domain.Entities.ForumReply>(forumreply);
            }
        }
    }
}
