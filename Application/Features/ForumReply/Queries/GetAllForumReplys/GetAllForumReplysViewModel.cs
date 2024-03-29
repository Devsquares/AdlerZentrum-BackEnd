using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities;

namespace Application.Features
{
	public class GetAllForumReplysViewModel
	{
		public int Id { get; set; }
		public string WriterId { get; set; }
		public ApplicationUser Writer { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public int ForumCommentId { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
