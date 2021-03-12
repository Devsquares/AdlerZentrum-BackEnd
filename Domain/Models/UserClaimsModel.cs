using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class UserClaimsModel
    {
        public string userId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
       // List<ClaimsTypeModel> Claims { get; set; }
    }
}
