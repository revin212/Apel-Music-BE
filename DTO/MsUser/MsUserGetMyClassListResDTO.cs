namespace fs_12_team_1_BE.DTO.MsUser
{
    public class MsUserGetMyClassListResDTO
    {
        public Guid CourseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
