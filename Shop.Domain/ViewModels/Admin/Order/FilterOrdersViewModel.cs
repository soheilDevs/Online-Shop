using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Admin.Order
{
    public class FilterOrdersViewModel:BasePaging
    {
        public long? UserId { get; set; }
        public OrderStateFilter OrderStateFilter { get; set; }
        public List<Models.Orders.Order> Orders { get; set; }


        #region Methods

        public FilterOrdersViewModel SetOrders(List<Models.Orders.Order> orders)
        {
            this.Orders = orders;
            return this;
        }

        public FilterOrdersViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.CountForShowAfterAndBefore = paging.CountForShowAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }

    public enum OrderStateFilter
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "درخواست شده")]
        Requested,
        [Display(Name = "درحال بررسی")]
        Processing,
        [Display(Name = "ارسال شده")]
        Sent,
        [Display(Name = "لغو شده")]
        Cancel
    }
}