using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Settings;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.RefreshTokens = new List<RefreshToken>();
        }
        [MaxLength(85)]
        public override string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual bool Active { get; set; }
        public virtual bool Deleted { get; set; }
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Profilephoto { get; set; }
        public string Avatar { get; set; }
        public bool Banned { get; set; }
        public string BanComment { get; set; }
        public Address Address { get; set; }
        public IdentityRole Role { get; set; }
        public bool ChangePassword { get; set; }
        public Sublevel Sublevel { get; set; }
        public int? SublevelId { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool Disqualified { get; set; }
        public string DisqualifiedComment { get; set; }
        public int AdlerCardBalance { get; set; }
        public int? PlacmentTestId { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
