﻿using Application.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.DTOs.Account
{
    public class TestRegisterRequest
    { 
        public int count { get; set; }
        public string identifier { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }  
        public string Country { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Profilephoto { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; } 
        public int? PlacmentTestId { get; set; }
        public bool IsAdlerService { get; set; }
        public int? PromoCodeInstanceID { get; set; }
        public int? GroupDefinitionId { get; set; }
    }
}
