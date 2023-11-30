namespace fs_12_team_1_BE.DTO.TsOrderDetail
{
    public class TsOrderDetailDTOCheckoutCart
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid CourseId { get; set; }
        public DateOnly Jadwal {  get; set; }
        public bool IsChecked { get; set; }
    }
}
