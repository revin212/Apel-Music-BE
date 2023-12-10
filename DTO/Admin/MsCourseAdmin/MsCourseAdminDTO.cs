namespace fs_12_team_1_BE.DTO.Admin.MsCourseAdmin
{
    public class MsCourseAdminDTO
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsActivated { get; set; }
    }
}
