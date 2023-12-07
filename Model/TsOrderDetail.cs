namespace fs_12_team_1_BE.Model
{
    public class TsOrderDetail
    {
        public int? Id { get; set; }
        public int? OrderId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime Jadwal { get; set; }
        public double Harga { get; set; }
        public bool? IsActivated { get; set; }
    }
}
