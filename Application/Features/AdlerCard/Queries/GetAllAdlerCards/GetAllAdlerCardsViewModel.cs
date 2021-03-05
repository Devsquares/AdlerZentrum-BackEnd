using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllAdlerCardsViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Domain.Entities.AdlerCardsUnit AdlerCardsUnit { get; set; }
		public int AdlerCardsUnitId { get; set; }
		public Question Question { get; set; }
		public int QuestionId { get; set; }
		public int AllowedDuration { get; set; }
		public double TotalScore { get; set; }
		public int Status { get; set; }
		public int AdlerCardsTypeId { get; set; }
	}
}
