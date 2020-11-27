using Domain.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Account
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
    }
}
