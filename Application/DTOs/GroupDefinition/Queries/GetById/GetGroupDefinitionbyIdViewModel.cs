using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.DTOs.GroupDefinition.Queries.GetById
{
    public class GetGroupDefinitionbyIdViewModel
    {
        public Domain.Entities.GroupDefinition GroupDefinition { get; set; }
        public List<IGrouping<int, GroupConditionPromoCode>> PromoCodes { get; set; } = new List<IGrouping<int, GroupConditionPromoCode>>();
    }
}
