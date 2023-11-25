namespace fs_12_team_1_BE.DTO.MsUser
{
    public class LoginResponseDTO
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? TokenExpires { get; set; }
    }
}
