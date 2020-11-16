using Application.DTOs.Account;
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
        public virtual int AddressId { get; set; }
        public virtual int InvoiceAddressId { get; set; }
        public virtual int DeliveryAddressId { get; set; }
        public virtual string Language { get; set; }
        public virtual string FaxNumber { get; set; }
        public virtual string TelefonNumber { get; set; }

        public virtual string BusinessStatus { get; set; }
        public virtual string Title { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual string ContactTelefonNumber { get; set; }
        public virtual string ContactPhoneNumber { get; set; }
        public virtual string ContactFaxNumber { get; set; }
        public virtual string ContactLanguge { get; set; }
        public virtual string APE { get; set; }
        public virtual string SIRET { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string CommercialName { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
    }
}
