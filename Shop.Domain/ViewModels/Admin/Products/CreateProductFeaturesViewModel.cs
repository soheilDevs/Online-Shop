using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class CreateProductFeaturesViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public long ProductId { get; set; }

        [Display(Name = "ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200,ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Title { get; set; }

        [Display(Name = "مقدار ویژگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Value { get; set; }
    }

    public enum CreateProductFeaturesResult
    {
        Error,
        Success
    }
}