using Domain.Entities;

public class GetAdlerCardUnitsForStudentViewModel
{ 
    public int AdlerCardUnitId { get; set; }
    public string AdlerCardUnitName { get; set; }
    public string AdlerCardUnitImage { get; set; }
    public string AdlerCardUnitDescription { get; set; }
    public int AdlerCardUnitCount { get; set; }
    public int AdlerCardUnitAchievedCount { get; set; }
    public Level Levels { get; set; } 
}