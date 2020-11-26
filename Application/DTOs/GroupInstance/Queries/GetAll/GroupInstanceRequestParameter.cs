using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.GroupInstance.Queries.GetAll
{
    public class GroupInstanceRequestParameter : RequestParameter
    {
        public DateTime DateTimeFrom { get; set; }
        public DateTime DateTimeTo { get; set; }
        public GroupInstanceRequestParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.DateTimeFrom = DateTimeFrom;
            this.DateTimeTo = DateTimeTo;
        }
        public GroupInstanceRequestParameter(int pageNumber, int pageSize, DateTime DateTimeFrom, DateTime DateTimeTo)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.DateTimeFrom = DateTimeFrom;
            this.DateTimeTo = DateTimeTo;
        }
    }
}
