using System.Threading.Tasks;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Admin.Order;

namespace Shop.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CheckUserOrder(long userId);
        Task AddOrder(Order order);
        Task SaveChanges();
        Task<Order> GetOrderById(long orderId);
        Task<Order> GetOrderById(long orderId,long userId);

        Task<OrderDetail> CheckOrderDetail(long orderId, long productId);
        void UpdateOrderDetail(OrderDetail orderDetail);
        Task AddOrderDetail(OrderDetail orderDetail);

        Task<int> OrderSum(long orderId);

        void UpdateOrder(Order order);

        Task<Order> GetUserBasket(long orderId, long userId);
        Task<Order> GetUserBasket(long userId);
        Task<OrderDetail> GetOrderDetailById(long id);

        Task<ResultOrderStateViewModel> GetResultOrder();
        Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders);

        Task<Order> GetOrderDetail(long orderId);
    }
}