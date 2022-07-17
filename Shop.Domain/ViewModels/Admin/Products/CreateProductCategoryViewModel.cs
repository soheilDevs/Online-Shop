using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class CreateProductCategoryViewModel
    {
        #region Properties

        [Display(Name = "عنوان")] //اسم دسته بندی
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Title { get; set; }
        [Display(Name = "عنوان url")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string UrlName { get; set; }
        //[Display(Name = "تصویر")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        //public string ImageName { get; set; }

        #endregion


    }

    public enum CreteProductCategoryResult
    {
        IsExistUrlName,
        Success
    }
}