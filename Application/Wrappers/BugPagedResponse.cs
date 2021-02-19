using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class BugPagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }

        public BugPagedResponse(T data, int pageNumber, int pageSize, int count, string type, string status, string priority)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.data = data;
            this.Count = count;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
            this.Type = type;
            this.Priority = priority;
            this.Status = status;
        }
    }
}
