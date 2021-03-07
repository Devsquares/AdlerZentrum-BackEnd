using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumTopic.Queries.GetAllForumTopics
{
	public class GetAllForumTopicsViewModel
	{
		public int Id { get; set; }
		public ApplicationUser Writer { get; set; }
		public string WriterId { get; set; }
		public string Header { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public Enums.ForumType ForumType { get; set; }
		public GroupInstance GroupInstance { get; set; }
		public int GroupInstanceId { get; set; }
		public GroupDefinition GroupDefinition { get; set; }
		public int GroupDefinitionId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int ForumCommentsCount { get; set; }
	}
}
