using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ForumComment: AuditableBaseEntity
    {
		public ApplicationUser Writer { get; set; }
		public string WriterId { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public ForumTopic ForumTopic { get; set; }
		public int ForumTopicId { get; set; }
		public ICollection<ForumReply> ForumReplys { get; set; }
	}
}
