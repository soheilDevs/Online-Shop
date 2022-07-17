using System.ComponentModel.DataAnnotations;
using Shop.Domain.ViewModels.Site;

namespace Shop.Domain.ViewModels.Account
{
    public class ActiveAccountViewModel:Recaptcha
    {

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "کد فعال سازی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(20, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string ActiveCode { get; set; }
    }

    public enum ActiveAccountResult
    {
        Error,
        Success,
        NotFound
    }
}