﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs.GroupInstance.Queries
{
    public class GetAllGroupInstancesViewModel
    {
        public string Id { get; set; }
        public int GroupDefinitionId { get; set; }
        public virtual Domain.Entities.GroupDefinition GroupDefinition { get; set; }
        public string Serial { get; set; }
        public int? Status { get; set; }

    }
}
