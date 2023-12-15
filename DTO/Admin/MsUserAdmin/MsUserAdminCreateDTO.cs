namespace fs_12_team_1_BE.DTO.Admin.MsUserAdmin
{
    public class MsUserAdminCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActivated { get; set; }
    }
}
