using System;
using System.Collections;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    /**
     */
    public class AdlerCard : AuditableBaseEntity
    {
        public string Name { get; set; }
        public AdlerCardsUnit AdlerCardsUnit { get; set; }
        public int AdlerCardsUnitId { get; set; }
        public Question Question { get; set; } //One-To-One Relation //QUestion1: Use Same table for adlercardsquestions? with adding a "isAdlerService" boolean?
        public int QuestionId { get; set; }
        public int AllowedDuration { get; set; }
        public double TotalScore { get; set; }
        public int Status { get; set; }

    }
}
