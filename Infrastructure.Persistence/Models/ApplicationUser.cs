using Application.DTOs.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual bool Active { get; set; }
        public virtual bool Deleted { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Profilephoto { get; set; }
        public string Avatar { get; set; }
        public Address Address { get; set; }

        public string Phone { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
