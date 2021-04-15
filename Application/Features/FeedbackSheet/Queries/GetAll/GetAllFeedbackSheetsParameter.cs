using Application.Enums;
using Application.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class GetAllFeedbackSheetsParameter : RequestParameter
    {
        public int GroupInstanceId { get; set; }
        public string StudentName { get; set; }
        public TestInstanceEnum Status { get; set; }
        public int LessonInstanceId { get; set; }

        public GetAllFeedbackSheetsParameter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.Status = 0;
            this.GroupInstanceId = 0;
            this.LessonInstanceId = 0;
            this.StudentName = String.Empty;
        }
        public GetAllFeedbackSheetsParameter(int pageNumber, int pageSize, TestInstanceEnum status, int groupInstanceId, int lessonInstanceId, string StudentName)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 || pageSize < 1 ? 10 : pageSize;
            this.Status = status;
            this.GroupInstanceId = groupInstanceId;
            this.LessonInstanceId = lessonInstanceId;
            this.StudentName = StudentName;
        }
    }
}
