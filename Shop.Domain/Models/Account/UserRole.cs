using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.Account
{
    public class UserRole:BaseEntity
    {
        #region Properties

        public long UserId { get; set; }
        public long RoleId { get; set; }

        #endregion

        #region Relations

        public User User { get; set; }
        public Role Role { get; set; }
        

        #endregion
    }
}