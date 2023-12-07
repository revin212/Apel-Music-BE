namespace fs_12_team_1_BE.DTO.TsOrderDetail
{
    public class TsOrderDetailGetCartListResDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid CourseId { get; set; }
        public string Image {  get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CourseName { get; set; } = string.Empty;
        public DateOnly Jadwal { get; set; }
        public double Harga { get; set; }
        public bool? IsActivated { get; set; }
        public bool? IsSelected { get; set; }
    }
}
