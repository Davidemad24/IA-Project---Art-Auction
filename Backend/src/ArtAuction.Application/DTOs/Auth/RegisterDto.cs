using System;
using System.Collections.Generic;
using System.Text;

namespace ArtAuction.Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}
