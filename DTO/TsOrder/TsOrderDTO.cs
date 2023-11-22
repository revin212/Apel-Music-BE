namespace fs_12_team_1_BE.DTO.TsOrder
{
    public class TsOrderDTO
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public bool IsPaid { get; set; }

    }
}
