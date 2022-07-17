namespace Shop.Domain.ViewModels
{
    public class FinallyOrderViewModel
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public bool IsWallet { get; set; }
    }

    public enum FinallyOrderResult
    {
        HasNotUser,
        NotFound,
        Error,
        Success
    }
}