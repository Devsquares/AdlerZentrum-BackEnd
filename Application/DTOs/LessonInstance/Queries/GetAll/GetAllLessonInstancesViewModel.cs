using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class GetAllLessonInstancesViewModel
    {
        public string Id { get; set; }
        public int GroupDefinitionId { get; set; }
        public virtual Domain.Entities.GroupDefinition GroupDefinition { get; set; }
        public int Serial { get; set; }
        public int? Status { get; set; }

    }
}
