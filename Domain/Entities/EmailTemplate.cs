using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class EmailTemplate : AuditableBaseEntity
    {
        public int EmailTypeId { get; set; }
        public virtual EmailType EmailType { get; set; }
        public string TemplateName { get; set; }
        public string TemplateBody { get; set; }
    }
}
