using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class GetAllUsersViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Profilephoto { get; set; }
        public string Country { get; set; }
    }
}
