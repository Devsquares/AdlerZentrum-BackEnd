using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Bug.Queries.GetAllBugs
{
    public class GetAllBugsParameter : RequestParameter
    {
        public string Type { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public GetAllBugsParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.Status = null;
            this.Priority = null;
            this.Status = null;
        }
        public GetAllBugsParameter(int pageNumber, int pageSize, string type, string priority, string status)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 || pageSize < 1 ? 10 : pageSize;
            this.Status = status;
            this.Priority = priority;
            this.Type = type;
        }
    }
}
