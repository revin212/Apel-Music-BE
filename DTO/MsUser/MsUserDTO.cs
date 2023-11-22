namespace fs_12_team_1_BE.DTO.MsUser
{
    public class MsUserDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
