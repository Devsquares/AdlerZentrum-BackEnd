using Application.DTOs.Account;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class LessonInstanceViewModel
    {
        public int Id { get; set; }
        public virtual GroupInstanceViewModel GroupInstance { get; set; }
        public int LessonDefinitionId { get; set; }
        public virtual LessonDefinition LessonDefinition { get; set; }
        public string MaterialDone { get; set; }
        public string MaterialToDo { get; set; }
        public string Serial { get; set; }
        public bool SubmittedReport { get; set; }
        public int? HomeWorkId { get; set; }
        public Homework Homework { get; set; }
        public virtual IList<LessonInstanceStudentViewModel> LessonInstanceStudents { get; set; }
    }
    public class GroupInstanceViewModel
    {
        public int Id { get; set; }
        public string Serial { get; set; }
        public int? Status { get; set; }
    }
    public class LessonInstanceStudentViewModel
    {
        public int LessonInstanceId { get; set; }
        public string StudentId { get; set; }
        public virtual AccountViewModel Student { get; set; }
        public bool Attend { get; set; }
        public bool Homework { get; set; }
        public bool Disqualified { get; set; }
    }
}
