using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class ForumTopicPagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public ForumType ForumType { get; set; }
        public int GroupInstanceId { get; set; }
        public int GroupDefinitionId { get; set; }
        public string UserId { get; set; }

        public ForumTopicPagedResponse(T data, int pageNumber, int pageSize, int count, string userId, ForumType forumType, int groupInstanceId, int groupDefinitionId)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.data = data;
            this.Count = count;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
            this.UserId = userId;
            this.ForumType = forumType;
            this.GroupInstanceId = groupInstanceId;
            this.GroupDefinitionId = groupDefinitionId;
        }
    }
}
