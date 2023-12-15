namespace fs_12_team_1_BE.DTO.TsOrderDetail
{
    public class TsOrderDetailUpdateSelectedCartItemDTO
    {
        public int CartItemId { get; set; }
        public bool IsSelected { get; set; }
        public Guid UserId { get; set; }
    }
}
