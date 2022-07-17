using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Admin.Account
{
    public class CreateOrEditRoleViewModel
    {
        public long Id { get; set; }

        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string RoleTitle { get; set; }

        public List<long> SelectedPermissions { get; set; }
    }

    public enum CreteOrEditRoleResult
    {
        NotFound,
        Success,
        NotExistPermissions
    }
}