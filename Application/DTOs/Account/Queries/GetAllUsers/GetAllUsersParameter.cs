using Application.Enums;
using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.Account.Queries.GetAllUsers
{
    public class GetAllUsersParameter : RequestParameter
    {
        public string Role { get; set; }
        public GetAllUsersParameter()
        {
            this.Role = Roles.Basic.ToString();
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public GetAllUsersParameter(string role, int pageNumber, int pageSize)
        {
            this.Role = role;
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}
