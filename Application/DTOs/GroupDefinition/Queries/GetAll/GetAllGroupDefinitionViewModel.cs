﻿using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;

namespace Application.DTOs
{
    public class GetAllGroupDefinitionViewModel
    {
        public int Id { get; set; }
        public int SubLevelId { get; set; }

        public virtual Sublevel Sublevel { get; set; }

        public int TimeSlotId { get; set; }

        public virtual TimeSlot TimeSlot { get; set; }

        public int PricingId { get; set; }

        public Pricing Pricing { get; set; }

        public int GroupConditionId { get; set; }

        public GroupCondition GroupCondition { get; set; }

        public double Discount { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime? FinalTestDate { get; set; }

        public int MaxInstances { get; set; }

        public string Serial { get; set; }

        public int? Status { get; set; }

        public int ActualTotalStudents { get; set; }
        public int ActualTotalGroupInstances { get; set; }
        public int TotalOverPaymentStudents { get; set; }
        public int TotalInterestedStudents { get; set; }
        public List<List<PromoCodeCountModel>> PromoCodes { get; set; } = new List<List<PromoCodeCountModel>>();
    }
}
