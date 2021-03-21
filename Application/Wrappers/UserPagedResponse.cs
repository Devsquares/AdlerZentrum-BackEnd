using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class UserPagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Role { get; set; }
        public int Count { get; set; }

        public UserPagedResponse(T data, string role, int pageNumber, int pageSize, int count = 10)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Role = role;
            this.Count = count;
            this.data = data;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
        }
    }
}
