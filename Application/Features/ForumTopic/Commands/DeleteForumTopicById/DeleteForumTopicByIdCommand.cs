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
    public class DeleteForumTopicByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteForumTopicByIdCommandHandler : IRequestHandler<DeleteForumTopicByIdCommand, Response<int>>
        {
            private readonly IForumTopicRepositoryAsync _forumtopicRepository;
            public DeleteForumTopicByIdCommandHandler(IForumTopicRepositoryAsync forumtopicRepository)
            {
                _forumtopicRepository = forumtopicRepository;
            }
            public async Task<Response<int>> Handle(DeleteForumTopicByIdCommand command, CancellationToken cancellationToken)
            {
                var forumtopic = await _forumtopicRepository.GetByIdAsync(command.Id);
                if (forumtopic == null) throw new ApiException($"ForumTopic Not Found.");
                await _forumtopicRepository.DeleteAsync(forumtopic);
                return new Response<int>(forumtopic.Id);
            }
        }
    }
}
