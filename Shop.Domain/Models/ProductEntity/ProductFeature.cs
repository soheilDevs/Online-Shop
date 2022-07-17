using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.ProductEntity
{
    public class ProductFeature:BaseEntity
    {
        #region Properties
        public long ProductId { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string FeatureTitle { get; set; }
        [Display(Name = "مقدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string FeatureValue { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }

        #endregion
    }
}