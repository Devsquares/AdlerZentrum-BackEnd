using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllAdlerCardSubmissionsViewModel
	{
		public int Id { get; set; }
		public Domain.Entities.AdlerCard AdlerCard { get; set; }
		public int AdlerCardId { get; set; }
		public ApplicationUser Student { get; set; }
		public string StudentId { get; set; }
		public ApplicationUser Teacher { get; set; }
		public string TeacherId { get; set; }
		public int Status { get; set; }
		public double AchievedScore { get; set; }
	}
}
