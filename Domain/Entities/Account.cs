using Domain.Common;
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
        public int AddressId { get; set; }
        public int InvoiceAddressId { get; set; }
        public int DeliveryAddressId { get; set; }
        public string Language { get; set; }
        public string FaxNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string TelefonNumber { get; set; }

        public string BusinessStatus { get; set; }
        public string Title { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTelefonNumber { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactFaxNumber { get; set; }
        public string ContactLanguge { get; set; }
        public string APE { get; set; }
        public string SIRET { get; set; }
        public string CompanyName { get; set; }
        public string CommercialName { get; set; }

        public List<string> Roles { get; set; }
    }
}
