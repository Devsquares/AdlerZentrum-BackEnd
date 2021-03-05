using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ForumTopic: AuditableBaseEntity
    {
		public ApplicationUser Writer { get; set; }
		public string WriterId { get; set; }
		public string Header { get; set; }
		public string Text { get; set; }
		public byte[] Image { get; set; }
		public int ForumType { get; set; }
		public GroupInstance GroupInstance { get; set; }
		public int GroupInstanceId { get; set; }
		public GroupDefinition GroupDefinition { get; set; }
		public int GroupDefinitionId { get; set; }
    }
}
