using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ForumReply: AuditableBaseEntity
    {
		public ApplicationUser Writer { get; set; }
		public string WriterId { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public int ForumCommentId { get; set; }
		public ForumComment ForumComment { get; set; }
	}
}
