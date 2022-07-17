using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Orders;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.Models.Wallet;

namespace Shop.Domain.Models.Account
{
    public class User: BaseEntity
    {

        #region Properties

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string LastName { get; set; }

        [Display(Name = "شماره تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "گذرواژه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Password { get; set; }

        [Display(Name = "آواتار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Avatar { get; set; }

        [Display(Name = "مسدود شده / نشده")]
        public bool IsBlocked { get; set; }

        [Display(Name = "کد احرازهویت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string MobileActiveCode { get; set; }

        [Display(Name = "تایید شده / نشده")]
        public bool IsMobileActive { get; set; }

        [Display(Name = "جنسیت")]
        public UserGender UserGender { get; set; }

        #endregion

        #region Relations

        public ICollection<UserWallet> UserWallets { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<UserCompare> UserCompares { get; set; }
        public ICollection<UserFavorite> UserFavorites { get; set; }

        #endregion

    }

    public enum UserGender
    {
        [Display(Name = "آقا")]
        Male,
        [Display(Name = "خانم")]
        Female,
        [Display(Name = "نامشخص")]
        Unknown
    }
}