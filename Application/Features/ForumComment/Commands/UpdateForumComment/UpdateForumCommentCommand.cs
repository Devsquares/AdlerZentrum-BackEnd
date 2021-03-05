using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ForumComment.Commands.UpdateForumComment
{
	public class UpdateForumCommentCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }

        public class UpdateForumCommentCommandHandler : IRequestHandler<UpdateForumCommentCommand, Response<int>>
        {
            private readonly IForumCommentRepositoryAsync _forumcommentRepository;
            public UpdateForumCommentCommandHandler(IForumCommentRepositoryAsync forumcommentRepository)
            {
                _forumcommentRepository = forumcommentRepository;
            }
            public async Task<Response<int>> Handle(UpdateForumCommentCommand command, CancellationToken cancellationToken)
            {
                var forumcomment = await _forumcommentRepository.GetByIdAsync(command.Id);

                if (forumcomment == null)
                {
                    throw new ApiException($"ForumComment Not Found.");
                }
                else
                {
				    forumcomment.Text = command.Text;
				    forumcomment.Image = command.Image;
                    await _forumcommentRepository.UpdateAsync(forumcomment);
                    return new Response<int>(forumcomment.Id);
                }
            }
        }

    }
}
