using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.Account
{
    public class Role:BaseEntity
    {
        #region Properties
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200,ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string RoleTitle { get; set; }
        #endregion

        #region Relations

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}