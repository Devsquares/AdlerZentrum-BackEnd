using Domain.Common;

namespace Domain.Entities
{
    public class AdlerCard : AuditableBaseEntity
    {
        public string Name { get; set; }
        public AdlerCardsUnit AdlerCardsUnit { get; set; }
        public int AdlerCardsUnitId { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public int AllowedDuration { get; set; }
        public double TotalScore { get; set; }
        public int Status { get; set; }
        public int AdlerCardsTypeId { get; set; }
        public Level Level { get; set; }
        public int LevelId { get; set; }
    }
}
