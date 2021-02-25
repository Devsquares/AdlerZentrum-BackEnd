using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
	public class GetAllAdlerCardsBundlesViewModel
	{
		public int Id { get; set; }
		public int Count { get; set; }
		public string Name { get; set; }
		public double Price { get; set; }
		public double DiscountPrice { get; set; }
		public DateTime? AvailableFrom { get; set; }
		public  DateTime? AvailableTill { get; set; }
		public int Status { get; set; }
	}
}
