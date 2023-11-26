namespace fs_12_team_1_BE.DTO.TsOrderDetail
{
    public class TsOrderDetailDTOAddToCart
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid CourseId { get; set; }
        public bool? IsActivated { get; set; }
    }
}
