namespace fs_12_team_1_BE.DTO.MsCategory
{
    public class MsCategoryDetailResDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public string HeaderImage { get; set; } = string.Empty;
    }
}
