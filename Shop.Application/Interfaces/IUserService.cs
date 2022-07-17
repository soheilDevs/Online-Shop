using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;

namespace Shop.Application.Interfaces
{
    public interface IUserService
    {
        #region Account

        Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register);
        Task<LoginUserResult> LoginUser(LoginUserViewModel login);
        Task<User> GetUserByPhoneNumber(string phoneNumber);
        Task<ActiveAccountResult> ActiveAccount(ActiveAccountViewModel activeAccount);
        Task<User> GetUserById(long userId);
        bool CheckPermission(long permissionId, string phoneNumber);
        #endregion


        #region Profile

        Task<EditUserProfileViewModel> GetEditUserProfile(long userId);
        Task<EditUserProfileResult> EditProfile(long userId, IFormFile userAvatar,
            EditUserProfileViewModel editUserProfile);

        Task<ChangePasswordResult> ChangePassword(long userId, ChangePasswordViewModel changePassword);

        Task<bool> AddProductToFavorite(long productId, long userId);
        Task<bool> AddProductToCompare(long productId, long userId);
        Task<List<UserCompare>> GetUserCompares(long userId);
        Task<int> UserFavoriteCount(long userId);
        Task<List<UserFavorite>> GetUserFavorites(long userId);
        Task<bool> RemoveAllUserCompare(long userId);
        Task<bool> RemoveUserCompare(long id);

        Task<UserComparesViewModel> UserCompares(UserComparesViewModel userCompares);
        Task<UserFavoritesViewModel> UserFavorites(UserFavoritesViewModel userFavorites);
        #endregion

        #region Admin

        Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter);
        Task<EditUserFromAdmin> GetEditUserFromAdmin(long userId);
        Task<EditUserFromAdminResult> EditUserFromAdmin(EditUserFromAdmin editUser);
        Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId);
        Task<CreteOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleViewModel createOrEditRole);
        Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter);
        Task<List<Permission>> GetAllActivePermission();
        Task<List<Role>> GetAllActiveRoles();


        #endregion


    }
}