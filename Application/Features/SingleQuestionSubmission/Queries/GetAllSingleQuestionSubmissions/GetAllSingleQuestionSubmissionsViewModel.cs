using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllSingleQuestionSubmissionsViewModel
	{
		public int Id { get; set; }
		public string AnswerText { get; set; }
		public bool TrueOrFalseSubmission { get; set; }
		public string StudentId { get; set; }
		public ICollection<ChoiceSubmission> Choices { get; set; }
		public ApplicationUser Student { get; set; }
		public SingleQuestion SingleQuestion { get; set; }
	}
}
