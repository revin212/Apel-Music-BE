using Newtonsoft.Json.Serialization;

namespace fs_12_team_1_BE.DTO.TsOrder
{
    public class TsOrderDTOCheckout
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentId { get; set; }
        //public string InvoiceNo { get; set; } = string.Empty;
    }
}
