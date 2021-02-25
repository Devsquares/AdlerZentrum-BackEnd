using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllAdlerCardsUnitsViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Level Level { get; set; }
		public int LevelId { get; set; }
		public int AdlerCardsTypeId { get; set; }
		public int Order { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
	}
}
