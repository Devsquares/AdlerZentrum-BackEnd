using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs
{
    public class BanRequestDTO
    {
        public string Id { get; set; }
        public string Comment { get; set; }
    }
}
