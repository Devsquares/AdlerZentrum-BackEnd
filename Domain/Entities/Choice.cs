using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Choice : AuditableBaseEntity
    {
        public string Name { get; set; }
        public int SingleQuestionId { get; set; }
        public bool IsCorrect {get;set;}
    }
}
