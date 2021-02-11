using Domain.Entities;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.DTOs.GroupDefinition.Queries.GetById
{
    public class GetGroupDefinitionbyIdViewModel
    {
        public Domain.Entities.GroupDefinition GroupDefinition { get; set; }
        public List<List<PromoCodeCountModel>> PromoCodes { get; set; } = new List<List<PromoCodeCountModel>>();
    }
}
