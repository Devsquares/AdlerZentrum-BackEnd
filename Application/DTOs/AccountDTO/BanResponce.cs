using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs
{
    public class BanResponce
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
