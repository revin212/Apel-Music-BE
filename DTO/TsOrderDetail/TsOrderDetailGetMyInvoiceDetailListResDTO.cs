namespace fs_12_team_1_BE.DTO.TsOrderDetail
{
    public class TsOrderDetailGetMyInvoiceDetailListResDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid CourseId { get; set; }
        public string CourseName {  get; set; } = string.Empty;
        public Guid CourseCategoryId { get; set; }
        public string CourseCategoryName { get; set; } = string.Empty;
        public DateTime Jadwal { get; set; }
        public double Harga { get; set; }
        public bool IsActivated { get; set; }
    }
}
