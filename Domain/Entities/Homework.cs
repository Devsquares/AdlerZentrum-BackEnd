using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Homework : AuditableBaseEntity
    {
        public string Text { get; set; }
        public int MinCharacters { get; set; }
        public int Points { get; set; }
        public int BonusPoints { get; set; }
        public int BonusPointsStatus { get; set; }
        public int GroupInstanceId { get; set; }
        public GroupInstance GroupInstance { get; set; }
        public int LessonInstanceId { get; set; }
        public virtual LessonInstance LessonInstance { get; set; }
        public string TeacherId { get; set; }
        public ApplicationUser Teacher { get; set; }
    }
}
