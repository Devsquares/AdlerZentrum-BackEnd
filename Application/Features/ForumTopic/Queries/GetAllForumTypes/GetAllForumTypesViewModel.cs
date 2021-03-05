using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumType.Queries.GetAllForumTypes
{
	public class GetAllForumTypesViewModel
	{
		public int ForumType { get; set; }
		public string Description { get; set; }
	}
}
