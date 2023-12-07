﻿using Org.BouncyCastle.Bcpg;
using System.Security.Permissions;

namespace fs_12_team_1_BE.DTO.MsUser
{
    public class LoginResponseDTO
    {
        public string? Token { get; set; }
        public DateTime? TokenExpires { get; set; }
        public string? UserId { get; set; }
    }
}
