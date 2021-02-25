using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features
{
    public class GetAdlerCardGroupsForStudentViewModel
    {
        public int LevelId { get; set; }
        public string LevelName { get; set; }
        public int AdlerCardTypeId { get; set; }
        public int NoOfCards { get; set; }
    }
}
