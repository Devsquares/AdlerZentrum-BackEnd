using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class QuestionDetails : AuditableBaseEntity
    {
        public int QuestionId { get; set; }
        public int SingleQuestionId { get; set; }
        public SingleQuestion SingleQuestion  { get; set; }
    }
}
