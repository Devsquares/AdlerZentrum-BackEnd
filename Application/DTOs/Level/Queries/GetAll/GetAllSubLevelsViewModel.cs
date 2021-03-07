namespace Application.DTOs
{
    public class GetAllSubLevelsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOflessons { get; set; }
        public string Color { get; set; }
        public bool IsFinal { get; set; }
        public int Quizpercent { get; set; }
        public int SublevelTestpercent { get; set; }
        public int FinalTestpercent { get; set; }
        public int LevelId { get; set; }
        public Domain.Entities.Level Level { get; set; }
    }
}