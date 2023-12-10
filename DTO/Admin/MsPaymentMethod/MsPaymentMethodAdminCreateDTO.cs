namespace fs_12_team_1_BE.DTO.Admin.MsPaymentMethod
{
    public class MsPaymentMethodAdminCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsActivated { get; set; }
    }
}
