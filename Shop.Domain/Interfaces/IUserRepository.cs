using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;

namespace Shop.Domain.Interfaces
{
    public interface IUserRepository
    {
        #region Account

        Task<bool> IsUserExistPhoneNumber(string phoneNumber);
        Task CreateUser(User user);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<User> GetUserById(long userId);
        void UpdateUser(User user);
        Task SaveChange();
        bool CheckPermission(long permissionId, string phoneNumber);

        Task<bool> IsExistProductFavorite(long productId, long userId);
        Task<bool> IsExistProductCompare(long productId, long userId);
        Task AddUserFavorite(UserFavorite userFavorite);
        Task AddUserCompare(UserCompare userCompare);
        Task<List<UserCompare>> GetUserCompares(long userId);
        Task<int> UserFavoriteCount(long userId);
        Task<List<UserFavorite>> GetUserFavorites(long userId);
        void UpdateUserCompare(UserCompare userCompare);
        Task RemoveProductInUserCompare(long id);
        Task<UserCompare> GetUSerCompare(long userId,long productId);

        Task RemoveAllRangeUserCompare(long userId);

        Task<UserComparesViewModel> UserCompares(UserComparesViewModel userCompares);
        Task<UserFavoritesViewModel> UserFavorites(UserFavoritesViewModel userFavorites);
        #endregion


        #region Admin
        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);
        Task<EditUserFromAdmin> GetEditUserFromAdmin(long userId);
        Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId);
        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter);
        Task CreateRole(Role role);
        void UpdateRole(Role role);
        Task<List<Permission>> GetAllActivePermission();

        Task<Role> GetRoleById(long id);
        Task RemoveAllPermissionSelectRole(long roleId);
        Task AddPermissionToRole(List<long> selectedPermission, long roleId);

        Task<List<Role>> GetAllActiveRoles();
        Task RemoveAllUserSelectRole(long userId);
        Task AddUserToRole(List<long> selectedRole, long userId);

        #endregion
    }
}