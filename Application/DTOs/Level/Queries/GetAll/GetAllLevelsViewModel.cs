using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class GetAllLevelsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ICollection<SubLevelsViewModel> SubLevels { get; set; }
      // public ICollection<Sublevel> SubLevels { get; set; }

    }
}
