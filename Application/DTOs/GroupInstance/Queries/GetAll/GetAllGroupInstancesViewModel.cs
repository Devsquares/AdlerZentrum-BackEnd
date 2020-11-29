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
        public virtual GroupDefinition GroupDefinition { get; set; }
        public int Serail { get; set; }
        public int? Status { get; set; }

    }
}
