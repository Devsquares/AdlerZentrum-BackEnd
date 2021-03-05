using Application.Enums;
using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumComment.Queries.GetAllForumComments
{
    public class GetAllForumCommentsParameter : RequestParameter
    {
        public int ForumTopicId { get; set; }
        public GetAllForumCommentsParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.ForumTopicId = 0;
        }
        public GetAllForumCommentsParameter(int pageNumber, int pageSize, int forumTopicId)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 || pageSize < 1 ? 10 : pageSize;
            this.ForumTopicId = forumTopicId;
        }
    }
}
