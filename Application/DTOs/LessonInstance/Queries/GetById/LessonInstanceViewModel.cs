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
    }
    public class GroupInstanceViewModel
    {
        public int Id { get; set; }
        public string Serial { get; set; }
        public int? Status { get; set; }
    }
}
