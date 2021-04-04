using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs
{
    public class SendMessageInputModel
    {
        public string Subject { get; set; }
        public string Message { get; set; }
        public string StudentId { get; set; } 
    }
}
