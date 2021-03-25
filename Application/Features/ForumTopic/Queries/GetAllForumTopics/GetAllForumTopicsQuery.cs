using Application.Enums;
using Application.Filters;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features
{
    public class GetAllForumTopicsQuery : IRequest<ForumTopicPagedResponse<IEnumerable<GetAllForumTopicsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Enums.ForumType ForumType { get; set; }
        public int GroupInstanceId { get; set; }
        public int GroupDefinitionId { get; set; }
        public string UserId { get; set; }
    }
    public class GetAllForumTopicsQueryHandler : IRequestHandler<GetAllForumTopicsQuery, ForumTopicPagedResponse<IEnumerable<GetAllForumTopicsViewModel>>>
    {
        private readonly IForumTopicRepositoryAsync _forumtopicRepository;
        private readonly IMapper _mapper;
        public GetAllForumTopicsQueryHandler(IForumTopicRepositoryAsync forumtopicRepository, IMapper mapper)
        {
            _forumtopicRepository = forumtopicRepository;
            _mapper = mapper;
        }

        public async Task<ForumTopicPagedResponse<IEnumerable<GetAllForumTopicsViewModel>>> Handle(GetAllForumTopicsQuery request, CancellationToken cancellationToken)
        {
            int count = _forumtopicRepository.GetCount(request.UserId, request.ForumType, request.GroupInstanceId, request.GroupDefinitionId);
            var forumtopic = await _forumtopicRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.UserId, request.ForumType, request.GroupInstanceId, request.GroupDefinitionId);
            var forumtopicViewModel = _mapper.Map<IEnumerable<GetAllForumTopicsViewModel>>(forumtopic);
            return new Wrappers.ForumTopicPagedResponse<IEnumerable<GetAllForumTopicsViewModel>>(forumtopicViewModel, request.PageNumber, request.PageSize, count, request.UserId, request.ForumType, request.GroupInstanceId, request.GroupDefinitionId);
        }
    }
}
