using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class PromoCodeCountModel
    {
        public int? promocodeId { get; set; }
        public int count { get; set; }
        public string PromoCodeName { get; set; }
    }
}
