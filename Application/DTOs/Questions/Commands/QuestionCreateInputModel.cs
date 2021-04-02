using System.Collections.Generic;

public class QuestionCreateInputModel
{
    public int Id { get; set; }
    public int QuestionTypeId { get; set; }
    public int Order { get; set; }
    public string Text { get; set; }
    public int MinCharacters { get; set; }
    public string AudioPath { get; set; }
    public int NoOfRepeats { get; set; }
    public virtual ICollection<SingleQuestionCreateInputModel> SingleQuestions { get; set; }
}