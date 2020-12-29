using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllChoiceSubmissionsViewModel
	{
		public int Id { get; set; }
		public Choice Choice { get; set; }
		public int ChoiceId { get; set; }
		public SingleQuestionSubmission SingleQuestionSubmission { get; set; }
		public int SingleQuestionSubmissionId { get; set; }
	}
}
