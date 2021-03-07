using Application.Features.ForumReply.Queries.GetAllForumReplys;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.ForumComment.Queries.GetAllForumComments
{
	public class GetAllForumCommentsViewModel
	{
		public int Id { get; set; }
		public ApplicationUser Writer { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public int ForumTopicId { get; set; }
		public string WriterId { get; set; }
		public ICollection<GetAllForumReplysViewModel> ForumReplys { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int ForumReplysCount { get; set; }
	}
}
