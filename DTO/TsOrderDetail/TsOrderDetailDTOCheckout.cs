using fs_12_team_1_BE.DTO.TsOrder;

namespace fs_12_team_1_BE.DTO.TsOrderDetail
{
    public class TsOrderDetailDTOCheckout
    {
        public TsOrderDTOCheckout CartInfo { get; set; } = new TsOrderDTOCheckout();
        public List<TsOrderDetailDTOCheckoutCart> CartItem { get; set; } = new List<TsOrderDetailDTOCheckoutCart> ();

    }
}
