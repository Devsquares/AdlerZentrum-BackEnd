using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllSublevelsViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int LevelId { get; set; }
		public virtual Level Level { get; set; }
		public int NumberOflessons { get; set; }
		public string Color { get; set; }
		public bool IsFinal { get; set; }
		public int Quizpercent { get; set; }
		public int SublevelTestpercent { get; set; }
		public int FinalTestpercent { get; set; }
		public List<LessonDefinition> LessonDefinitions { get; set; }
	}
}
