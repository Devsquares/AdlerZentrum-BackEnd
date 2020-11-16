using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Account
{
    public class RegisterBusinessRequest
    {

        [Required]
        public string CompanyName { get; set; }

        [Required]
        public string CommercialName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string APE { get; set; }

        [Required]
        public string SIRET { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string TelefonNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string PostCode { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string ContactEmail { get; set; }

        [Required]
        public string ContactTelefonNumber { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string ContactFaxNumber { get; set; }

        [Required]
        public string ContactLanguge { get; set; }




    }
}
