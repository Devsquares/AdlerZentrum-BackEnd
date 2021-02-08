using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? ActiveGroupInstance { get; set; }
        public List<string> Roles { get; set; }
        public bool IsVerified { get; set; }
        public string JWToken { get; set; }
        public bool ChangePassword { get; set; }
        public bool Banned { get; set; }
        public string BanComment { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public int? SubLevelId { get; set; }
    }
}
