using System.Threading.Tasks;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels;
using Shop.Domain.ViewModels.Admin.Order;

namespace Shop.Application.Interfaces
{
    public interface IOrderService
    {
        Task<long> AddOrder(long userId, long productId);

        Task UpdatePriceOrder(long orderId);

        Task<Order> GetUserBasket(long orderId, long userId);
        Task<Order> GetUserBasket(long userId);

        Task<FinallyOrderResult> FinallyOrder(FinallyOrderViewModel finallyOrder, long userId);

        Task<Order> GetOrderById(long orderId);

        Task<bool> RemoveOrderDetailFromOrder(long orderDetailId);

        Task ChangeIsFinallyToOrder(long orderId);

        Task<ResultOrderStateViewModel> GetResultOrder();
        Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders);

        Task<bool> ChangeStateToSent(long orderId);

        Task<Order> GetOrderDetail(long orderId);

    }
}