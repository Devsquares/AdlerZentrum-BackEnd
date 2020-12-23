using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs 
{
    public class GetAllPricingViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int? Status { get; set; }

    }
}
