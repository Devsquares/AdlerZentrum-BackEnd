using Application.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Wrappers
{
    public class FeedbackSheetPagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public int GroupInstanceId { get; set; }
        public string StudentName { get; set; }
        public TestInstanceEnum Status { get; set; }
        public int LessonInstanceId { get; set; }

        public FeedbackSheetPagedResponse(T data, int pageNumber, int pageSize, int count, int groupInstanceId, string StudentName, TestInstanceEnum status, int lessonInstanceId)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.data = data;
            this.Count = count;
            this.message = null;
            this.succeeded = true;
            this.errors = null;
            this.Status = status;
            this.GroupInstanceId = groupInstanceId;
            this.LessonInstanceId = lessonInstanceId;
            this.StudentName = StudentName;
        }
    }
}
