using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize, int count = 10)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.data = data;
            this.Count = count;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
        }
    }
}
