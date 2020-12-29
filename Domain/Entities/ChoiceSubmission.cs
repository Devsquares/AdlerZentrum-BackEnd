using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ChoiceSubmission : AuditableBaseEntity
    {
        public Choice Choice { get; set; }
        public int ChoiceId { get; set; }
        public SingleQuestionSubmission SingleQuestionSubmission { get; set; }
        public int SingleQuestionSubmissionId { get; set; }
    }
}
