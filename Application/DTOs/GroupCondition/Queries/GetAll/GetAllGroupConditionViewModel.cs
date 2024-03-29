﻿using Application.DTOs.GroupConditionPromoCodeModel;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class GetAllGroupConditionViewModel
    {
        public int Id { get; set; }
        public int? Status { get; set; }
        public int NumberOfSlots { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }
        public string Name { get; set; }
        public List<List<GroupConditionPromoCodeInputModel>> PromoCodes { get; set; } = new List<List<GroupConditionPromoCodeInputModel>>();

    }
}
