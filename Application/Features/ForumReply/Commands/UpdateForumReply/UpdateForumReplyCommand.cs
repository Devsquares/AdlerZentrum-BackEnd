using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ForumReply.Commands.UpdateForumReply
{
	public class UpdateForumReplyCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }

        public class UpdateForumReplyCommandHandler : IRequestHandler<UpdateForumReplyCommand, Response<int>>
        {
            private readonly IForumReplyRepositoryAsync _forumreplyRepository;
            public UpdateForumReplyCommandHandler(IForumReplyRepositoryAsync forumreplyRepository)
            {
                _forumreplyRepository = forumreplyRepository;
            }
            public async Task<Response<int>> Handle(UpdateForumReplyCommand command, CancellationToken cancellationToken)
            {
                var forumreply = await _forumreplyRepository.GetByIdAsync(command.Id);

                if (forumreply == null)
                {
                    throw new ApiException($"ForumReply Not Found.");
                }
                else
                {
				    forumreply.Text = command.Text;
				    forumreply.Image = command.Image;

                    await _forumreplyRepository.UpdateAsync(forumreply);
                    return new Response<int>(forumreply.Id);
                }
            }
        }

    }
}
