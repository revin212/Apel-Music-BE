namespace fs_12_team_1_BE.Model
{
    public class MsUser
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActivated { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
