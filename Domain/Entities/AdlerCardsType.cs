using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /**
     * Example: Writing, Speaking, Listening, Reading
     * QUestion1: Use this or your QUestionTypeENum?
     */
    public class AdlerCardsType : AuditableBaseEntity
    {
        public string Name { get; set; }

    }
}
