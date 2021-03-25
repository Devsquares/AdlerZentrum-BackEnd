using Application.Enums;
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
	public class UpdateForumTopicCommand : IRequest<Response<int>>
    {
		public int Id { get; set; }
		public string Header { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public Enums.ForumType ForumType { get; set; }
		public int GroupInstanceId { get; set; }
		public int GroupDefinitionId { get; set; }

        public class UpdateForumTopicCommandHandler : IRequestHandler<UpdateForumTopicCommand, Response<int>>
        {
            private readonly IForumTopicRepositoryAsync _forumtopicRepository;
            public UpdateForumTopicCommandHandler(IForumTopicRepositoryAsync forumtopicRepository)
            {
                _forumtopicRepository = forumtopicRepository;
            }
            public async Task<Response<int>> Handle(UpdateForumTopicCommand command, CancellationToken cancellationToken)
            {
                var forumtopic = await _forumtopicRepository.GetByIdAsync(command.Id);

                if (forumtopic == null)
                {
                    throw new ApiException($"ForumTopic Not Found.");
                }
                else
                {
				    forumtopic.Header = command.Header;
				    forumtopic.Text = command.Text;
				    forumtopic.Image = command.Image;
				    forumtopic.GroupInstanceId = command.GroupInstanceId;
				    forumtopic.GroupDefinitionId = command.GroupDefinitionId;

                    await _forumtopicRepository.UpdateAsync(forumtopic);
                    return new Response<int>(forumtopic.Id);
                }
            }
        }

    }
}
