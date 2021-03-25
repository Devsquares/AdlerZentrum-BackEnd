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
    public class DeleteForumReplyByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteForumReplyByIdCommandHandler : IRequestHandler<DeleteForumReplyByIdCommand, Response<int>>
        {
            private readonly IForumReplyRepositoryAsync _forumreplyRepository;
            public DeleteForumReplyByIdCommandHandler(IForumReplyRepositoryAsync forumreplyRepository)
            {
                _forumreplyRepository = forumreplyRepository;
            }
            public async Task<Response<int>> Handle(DeleteForumReplyByIdCommand command, CancellationToken cancellationToken)
            {
                var forumreply = await _forumreplyRepository.GetByIdAsync(command.Id);
                if (forumreply == null) throw new ApiException($"ForumReply Not Found.");
                await _forumreplyRepository.DeleteAsync(forumreply);
                return new Response<int>(forumreply.Id);
            }
        }
    }
}
