﻿using Microsoft.AspNetCore.Identity;

namespace E_Commerce.Models
{
    public class User : IdentityUser
    {
        public DateTime LastLoginTime { get; set; }
    }
}
