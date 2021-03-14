using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class UserPhotoModel
    {
        public string UserId { get; set; }
        public string base64photo { get; set; }
    }
}
