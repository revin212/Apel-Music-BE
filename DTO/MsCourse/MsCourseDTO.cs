namespace fs_12_team_1_BE.DTO.MsCourse
{
    public class MsCourseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public Guid CategoryId { get; set; }
    }
}
