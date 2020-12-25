using Domain.Common;

namespace Domain.Entities
{
    public class ChoiceSubmission : AuditableBaseEntity
    {
        public Choice Choice { get; set; }

        public SingleQuestionSubmission SingleQuestionSubmtion { get; set; }
    }
}
