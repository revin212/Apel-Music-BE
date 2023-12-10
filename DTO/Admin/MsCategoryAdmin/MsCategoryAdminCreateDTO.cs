namespace fs_12_team_1_BE.DTO.Admin.MsCategoryAdmin
{
    public class MsCategoryAdminCreateDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string HeaderImage { get; set; } = string.Empty;
        public bool IsActivated { get; set; }
    }
}
