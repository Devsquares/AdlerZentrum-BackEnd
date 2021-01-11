using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ChoiceSubmission : AuditableBaseEntity
    {
        public int ChoiceSubmissionId { get; set; }
        public int SingleQuestionSubmissionId { get; set; }
        public SingleQuestionSubmission SingleQuestionSubmission { get; set; }
    }
}
