namespace fs_12_team_1_BE.DTO.TsOrder
{
    public class TsOrderAdminGetAllInvoiceListResDTO
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public int course_count { get; set; }
        public double TotalHarga { get; set; }
        public string PaymentName { get; set; } = string.Empty;
    }
}
