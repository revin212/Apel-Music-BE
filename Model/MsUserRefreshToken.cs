namespace fs_12_team_1_BE.Model
{
    public class MsUserRefreshToken
    {
        public Guid Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; } = DateTime.Now;
    }
}
