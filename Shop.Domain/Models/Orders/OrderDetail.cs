using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.ProductEntity;

namespace Shop.Domain.Models.Orders
{
    public class OrderDetail:BaseEntity 
    {
        #region Properties
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long OrderId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductId { get; set; }
        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Count { get; set; }
        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        #endregion

        #region Relations

        public Order Order { get; set; }
        public Product Product { get; set; }

        #endregion
    }
}