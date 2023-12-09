namespace fs_12_team_1_BE.DTO.MsUser
{
    public class RefreshTokenDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpires { get; set; } = DateTime.Now;
    }
}
