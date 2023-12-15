namespace fs_12_team_1_BE.DTO.Admin.MsPaymentMethod
{
    public class MsPaymentMethodAdminDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsActivated { get; set; }
    }
}
