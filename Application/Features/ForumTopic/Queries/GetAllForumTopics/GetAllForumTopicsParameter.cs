using Application.Enums;
using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumTopic.Queries.GetAllForumTopics
{
    public class GetAllForumTopicsParameter : RequestParameter
    {
        public Enums.ForumType ForumType { get; set; }
        public int GroupInstanceId { get; set; }
        public int GroupDefinitionId { get; set; }
        public GetAllForumTopicsParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.ForumType = 0;
            this.GroupInstanceId = 0;
            this.GroupDefinitionId = 0;
        }
        public GetAllForumTopicsParameter(int pageNumber, int pageSize, Enums.ForumType forumType, int groupInstanceId, int groupDefinitionId)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 || pageSize < 1 ? 10 : pageSize;
            this.ForumType = forumType;
            this.GroupInstanceId = groupInstanceId;
            this.GroupDefinitionId = groupDefinitionId;
        }
    }
}
