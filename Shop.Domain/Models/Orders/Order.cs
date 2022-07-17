using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.Orders
{
    public class Order:BaseEntity   
    {
        #region Properties
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int OrderSum { get; set; }
        public bool IsFinally { get; set; }
        public OrderState OrderState { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        #endregion
    }

    public enum OrderState
    {
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