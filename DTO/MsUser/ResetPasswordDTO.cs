﻿namespace fs_12_team_1_BE.DTO.MsUser
{
    public class ResetPasswordDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
