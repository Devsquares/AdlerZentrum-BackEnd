using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Bug: AuditableBaseEntity
    {
		public string UserName { get; set; }
		public string BugName { get; set; }
		public string Type { get; set; }
		public string Priority { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
    }
}
