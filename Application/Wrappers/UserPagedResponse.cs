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

        public UserPagedResponse(T data, string role, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Role = role;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
