using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

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
        public RolesEnum Role { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Profilephoto { get; set; }

        [Required]
        public Address Address { get; set; }

        [Required]
        public string PhoneNumber{ get; set; }

        [Required]
        public int GroupInstanceId { get; set; }
    }
}
