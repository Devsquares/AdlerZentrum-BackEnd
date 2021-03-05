using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ForumComment.Commands.DeleteForumCommentById
{
    public class DeleteForumCommentByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteForumCommentByIdCommandHandler : IRequestHandler<DeleteForumCommentByIdCommand, Response<int>>
        {
            private readonly IForumCommentRepositoryAsync _forumcommentRepository;
            public DeleteForumCommentByIdCommandHandler(IForumCommentRepositoryAsync forumcommentRepository)
            {
                _forumcommentRepository = forumcommentRepository;
            }
            public async Task<Response<int>> Handle(DeleteForumCommentByIdCommand command, CancellationToken cancellationToken)
            {
                var forumcomment = await _forumcommentRepository.GetByIdAsync(command.Id);
                if (forumcomment == null) throw new ApiException($"ForumComment Not Found.");
                await _forumcommentRepository.DeleteAsync(forumcomment);
                return new Response<int>(forumcomment.Id);
            }
        }
    }
}
