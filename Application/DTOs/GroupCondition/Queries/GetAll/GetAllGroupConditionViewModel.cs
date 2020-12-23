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
        public int NumberOfSolts { get; set; }
        public int NumberOfSlotsWithPlacementTest { get; set; }

    }
}
