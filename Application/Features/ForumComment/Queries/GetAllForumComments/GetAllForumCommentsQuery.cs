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

namespace Application.Features.ForumComment.Queries.GetAllForumComments
{
    public class GetAllForumCommentsQuery : IRequest<ForumCommentPagedResponse<IEnumerable<GetAllForumCommentsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ForumTopicId { get; set; }
    }
    public class GetAllForumCommentsQueryHandler : IRequestHandler<GetAllForumCommentsQuery, ForumCommentPagedResponse<IEnumerable<GetAllForumCommentsViewModel>>>
    {
        private readonly IForumCommentRepositoryAsync _forumcommentRepository;
        private readonly IMapper _mapper;
        public GetAllForumCommentsQueryHandler(IForumCommentRepositoryAsync forumcommentRepository, IMapper mapper)
        {
            _forumcommentRepository = forumcommentRepository;
            _mapper = mapper;
        }

        public async Task<ForumCommentPagedResponse<IEnumerable<GetAllForumCommentsViewModel>>> Handle(GetAllForumCommentsQuery request, CancellationToken cancellationToken)
        {
            int count = _forumcommentRepository.GetCount(request.ForumTopicId);
            var forumcomment = await _forumcommentRepository.GetPagedReponseAsync(request.PageNumber, request.PageSize, request.ForumTopicId);
            var forumcommentViewModel = _mapper.Map<IEnumerable<GetAllForumCommentsViewModel>>(forumcomment);
            return new Wrappers.ForumCommentPagedResponse<IEnumerable<GetAllForumCommentsViewModel>>(forumcommentViewModel, request.PageNumber, request.PageSize, count, request.ForumTopicId);
        }
    }
}
