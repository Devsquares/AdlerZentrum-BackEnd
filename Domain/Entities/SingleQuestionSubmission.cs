using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SingleQuestionSubmission : AuditableBaseEntity
    {
        public string AnswerText { get; set; }
        public string CorrectionText { get; set; }
        public bool TrueOrFalseSubmission { get; set; }
        public string StudentId { get; set; }
        public ICollection<ChoiceSubmission> Choices { get; set; }
        public ApplicationUser Student { get; set; }
        public int SingleQuestionId { get; set; }
        public SingleQuestion SingleQuestion { get; set; }
        public bool Corrected { get; set; }
        public bool RightAnswer { get; set; }
        public int Points { get; set; }
        public int TestInstanceId { get; set; }
        public TestInstance TestInstance { get; set; }
    }
}
