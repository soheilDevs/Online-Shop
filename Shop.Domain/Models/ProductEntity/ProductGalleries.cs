using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.ProductEntity
{
    public class ProductGalleries:BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }
        [Display(Name = "نصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string ImageName { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }

        #endregion
    }
}