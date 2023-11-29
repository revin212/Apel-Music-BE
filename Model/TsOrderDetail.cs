namespace fs_12_team_1_BE.Model
{
    public class TsOrderDetail
    {
        public Guid? Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid CourseId { get; set; }
        public DateOnly Jadwal { get; set; }
        public bool? IsActivated { get; set; }
    }
}
