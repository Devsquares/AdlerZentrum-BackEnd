using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class ForumCommentPagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public int ForumTopicId { get; set; }

        public ForumCommentPagedResponse(T data, int pageNumber, int pageSize, int count, int forumTopicId)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.data = data;
            this.Count = count;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
            this.ForumTopicId = forumTopicId;
        }
    }
}
