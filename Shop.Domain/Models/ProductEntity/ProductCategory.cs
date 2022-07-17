using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.ProductEntity
{
    public class ProductCategory:BaseEntity
    {
        #region Properties
        public long? ParentId { get; set; }
        [Display(Name = "عنوان")] //اسم دسته بندی
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200,ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Title { get; set; }
        [Display(Name = "عنوان url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string UrlName { get; set; }
        [Display(Name = "تصویر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string ImageName { get; set; }

        #endregion

        #region Relations

        public ProductCategory Category { get; set; }
        public ICollection<ProductSelectedCategories> ProductSelectedCategories { get; set; }

        #endregion
    }
}