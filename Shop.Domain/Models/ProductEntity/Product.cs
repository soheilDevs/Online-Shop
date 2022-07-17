using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Orders;

namespace Shop.Domain.Models.ProductEntity
{
    public class Product:BaseEntity
    {
        #region Properties
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500,ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Name { get; set; }
        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }
        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string ProductImageName { get; set; }
        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }

        #endregion

        #region Relations

        public ICollection<ProductGalleries> ProductGalleries { get; set; }
        public ICollection<ProductSelectedCategories> ProductSelectedCategories { get; set; }
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<UserCompare> UserCompares { get; set; }
        public ICollection<UserFavorite> UserFavorites { get; set; }

        #endregion
    }
}