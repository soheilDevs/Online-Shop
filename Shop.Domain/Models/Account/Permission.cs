using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.Account
{
    public class Permission:BaseEntity
    {
        #region Properties
        [Display(Name = "عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Title { get; set; }
        public long? ParentId { get; set; }
        #endregion

        #region Relations
        [ForeignKey("ParentId")]
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}