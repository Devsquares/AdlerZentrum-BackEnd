using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.ForumTopic.Queries.GetForumTopicById
{
    public class GetForumTopicByIdQuery : IRequest<Response<Domain.Entities.ForumTopic>>
    {
        public int Id { get; set; }
        public class GetForumTopicByIdQueryHandler : IRequestHandler<GetForumTopicByIdQuery, Response<Domain.Entities.ForumTopic>>
        {
            private readonly IForumTopicRepositoryAsync _forumtopicRepository;
            public GetForumTopicByIdQueryHandler(IForumTopicRepositoryAsync forumtopicRepository)
            {
                _forumtopicRepository = forumtopicRepository;
            }
            public async Task<Response<Domain.Entities.ForumTopic>> Handle(GetForumTopicByIdQuery query, CancellationToken cancellationToken)
            {
                var forumtopic = await _forumtopicRepository.GetByIdAsync(query.Id);
                if (forumtopic == null) throw new ApiException($"ForumTopic Not Found.");
                return new Response<Domain.Entities.ForumTopic>(forumtopic);
            }
        }
    }
}
