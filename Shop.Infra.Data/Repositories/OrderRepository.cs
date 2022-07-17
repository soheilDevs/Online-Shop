using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Orders;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Domain.ViewModels.Paging;
using Shop.Infra.Data.Context;

namespace Shop.Infra.Data.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        #region Constractor

        private readonly ShopDbContext _context;

        public OrderRepository(ShopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Order

        public async Task<Order> CheckUserOrder(long userId)
        {
            return await _context.Orders.AsQueryable()
                .FirstOrDefaultAsync(c => c.UserId == userId && !c.IsFinally);
        }

        public async Task AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderById(long orderId)
        {
            return await _context.Orders.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == orderId);
        }

        public async Task<Order> GetOrderById(long orderId, long userId)
        {
            return await _context.Orders.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == orderId && c.UserId==userId);
        }

        public async Task<OrderDetail> CheckOrderDetail(long orderId, long productId)
        {
            return await _context.OrderDetails.AsQueryable()
                .Where(c => c.OrderId == orderId && c.ProductId == productId && !c.IsDelete)
                .FirstOrDefaultAsync();
        }

        public void UpdateOrderDetail(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);
        }

        public async Task AddOrderDetail(OrderDetail orderDetail)
        {
            await _context.OrderDetails.AddAsync(orderDetail);
        }

        public async Task<int> OrderSum(long orderId)
        {
            return await _context.OrderDetails.AsQueryable()
                .Where(c => c.OrderId == orderId && !c.IsDelete).SumAsync(c => c.Price*c.Count);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
        }

        public async Task<Order> GetUserBasket(long orderId, long userId)
        {
            return await _context.Orders.AsQueryable()
                .Include(c=>c.User)
                .Where(c => c.UserId == userId && c.Id == orderId)
                .Select(c => new Order
                {
                    UserId = c.UserId,
                    CreateData = c.CreateData,
                    Id = c.Id,
                    IsDelete = c.IsDelete,
                    IsFinally = c.IsFinally,
                    OrderSum = c.OrderSum,
                    //OrderDetails = _context.OrderDetails.Include(c => c.Product)
                    //    .Where(c => !c.IsDelete && !c.Order.IsFinally && c.Order.UserId == userId)
                    //    .ToList()
                    OrderDetails = _context.OrderDetails.Where(c=>!c.IsDelete &&!c.Order.IsFinally&&c.Order.UserId==userId)
                        .Include(c=>c.Product).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<Order> GetUserBasket(long userId)
        {
            return await _context.Orders.Include(c => c.OrderDetails).ThenInclude(c => c.Product).AsQueryable()
                .Where(c => c.UserId == userId && c.OrderState == OrderState.Processing && !c.IsFinally && !c.IsDelete)
                .FirstOrDefaultAsync();
        }

        public async Task<OrderDetail> GetOrderDetailById(long id)
        {
          return  await _context.OrderDetails.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<ResultOrderStateViewModel> GetResultOrder()
        {
            //return await _context.Orders.AsQueryable()
            //    .Select(c=>new ResultOrderStateViewModel
            //    {
            //        CancelCount = 
            //    })

            return new ResultOrderStateViewModel
            {
                CancelCount = await _context.Orders.AsQueryable().Where(c=>c.OrderState==OrderState.Cancel && c.CreateData.Day == DateTime.Now.Day).CountAsync(),
                RequestCount = await _context.Orders.AsQueryable().Where(c=>c.OrderState==OrderState.Requested).CountAsync(),
                ProcessingCount = await _context.Orders.AsQueryable().Where(c=>c.OrderState==OrderState.Processing && c.CreateData.Day == DateTime.Now.Day).CountAsync(),
                SentCount = await _context.Orders.AsQueryable().Where(c=>c.OrderState==OrderState.Sent).CountAsync()
            };
        }

        public async Task<FilterOrdersViewModel> FilterOrders(FilterOrdersViewModel filterOrders)
        {
            var query = _context.Orders.Include(c => c.OrderDetails).Include(c=>c.User)
                .AsQueryable();

            #region filter

            if (filterOrders.UserId.HasValue &&filterOrders.UserId!=0)
            {
                query = query.Where(c => c.UserId == filterOrders.UserId);
            }
            #endregion

            #region state

            switch (filterOrders.OrderStateFilter)
            {
                case OrderStateFilter.All:
                    break;
                case OrderStateFilter.Requested:
                    query = query.Where(c => c.OrderState == OrderState.Requested);
                    break;
                case OrderStateFilter.Processing:
                    query = query.Where(c => c.OrderState == OrderState.Processing);
                    break;
                case OrderStateFilter.Sent:
                    query = query.Where(c => c.OrderState == OrderState.Sent);
                    break;
                case OrderStateFilter.Cancel:
                    query = query.Where(c => c.OrderState == OrderState.Cancel);
                    break;
            }

            #endregion
            var pager = Pager.Build(filterOrders.PageId, await query.CountAsync(), filterOrders.TakeEntity,
                filterOrders.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();


            return filterOrders.SetPaging(pager).SetOrders(allData);
        }

        public async Task<Order> GetOrderDetail(long orderId)
        {
            return await _context.Orders.Include(c=>c.OrderDetails).ThenInclude(c=>c.Product).Include(c=>c.User).AsQueryable()
                .Where(c => c.Id == orderId)
                .SingleOrDefaultAsync();
        }

        #endregion
    }
}