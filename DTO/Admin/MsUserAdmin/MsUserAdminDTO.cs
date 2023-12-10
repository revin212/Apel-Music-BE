namespace fs_12_team_1_BE.DTO.Admin.MsUserAdmin
{
    public class MsUserAdminDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public bool IsActivated { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RefreshToken {  get; set; } = string.Empty;
        public string RefreshTokenExpires { get; set; } = string.Empty;
    }
}
