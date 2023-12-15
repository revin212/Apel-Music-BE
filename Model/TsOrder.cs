namespace fs_12_team_1_BE.Model
{
    public class TsOrder
    {
        public int? Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? PaymentId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public int Course_Count { get; set; }
        public double TotalHarga { get; set; }
        public bool? IsPaid{ get; set; }
        
    }
}
