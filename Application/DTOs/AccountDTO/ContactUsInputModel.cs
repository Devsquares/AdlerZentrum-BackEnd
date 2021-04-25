using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs
{
    public class ContactUsInputModel
    {
        public string Subject { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
