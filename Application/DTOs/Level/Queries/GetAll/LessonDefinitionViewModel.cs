using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class LessonDefinitionViewModel
    {
        public int Id { get; set; }
        public int SublevelId { get; set; }
        public int Order { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
