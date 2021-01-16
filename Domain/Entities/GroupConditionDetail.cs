using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class GroupConditionDetail: AuditableBaseEntity
    {
	public int GroupConditionId { get; set; }
	public virtual GroupCondition GroupCondition { get; set; }
    }
}
