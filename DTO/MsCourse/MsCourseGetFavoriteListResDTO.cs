namespace fs_12_team_1_BE.DTO.MsCourse
{
    public class MsCourseGetFavoriteListResDTO
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
