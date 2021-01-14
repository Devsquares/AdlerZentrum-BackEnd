using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class EmailType: AuditableBaseEntity
    {
	public string TypeName { get; set; }
	public string Code { get; set; }
    }
}
