﻿using System.ComponentModel.DataAnnotations;

namespace Ecommerce2.Dtos
{
    public class UserForLoginConfirmationDto
    {
        [Key]
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}
