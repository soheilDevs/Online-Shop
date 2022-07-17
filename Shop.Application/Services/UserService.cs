﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;

namespace Shop.Application.Services
{
    public class UserService : IUserService
    {
        #region Constractor

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ISmsService _smsService;

        public UserService(IUserRepository userRepository, IPasswordHelper passwordHelper, ISmsService smsService)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _smsService = smsService;
        }

        #endregion

        #region Account

        public async Task<RegisterUserResult> RegisterUser(RegisterUserViewModel register)
        {
            if (!await _userRepository.IsUserExistPhoneNumber(register.PhoneNumber))
            {
                var user = new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    UserGender = UserGender.Unknown,
                    Password = _passwordHelper.EncodePasswordMd5(register.Password),
                    PhoneNumber = register.PhoneNumber,
                    Avatar = "default.png",
                    IsMobileActive = false,
                    MobileActiveCode = new Random().Next(10000, 99999).ToString(),
                    IsBlocked = false, //age nanevisi ham pishfarz false
                    IsDelete = false //age nanevisi ham pishfarz false

                };
                await _userRepository.CreateUser(user);
                await _userRepository.SaveChange();
                await _smsService.SendVerificationCode(user.PhoneNumber, user.MobileActiveCode);

                return RegisterUserResult.Success;
            }

            return RegisterUserResult.MobileExists;

        }

        public async Task<LoginUserResult> LoginUser(LoginUserViewModel login)
        {
            var user = await _userRepository.GetUserByPhoneNumber(login.PhoneNumber);
            if (user == null) return LoginUserResult.NotFound;
            if (user.IsBlocked) return LoginUserResult.IsBlocked;
            if (!user.IsMobileActive) return LoginUserResult.NotActive;
            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password)) return LoginUserResult.NotFound;

            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumber(phoneNumber);
        }

        public async Task<ActiveAccountResult> ActiveAccount(ActiveAccountViewModel activeAccount)
        {
            var user = await _userRepository.GetUserByPhoneNumber(activeAccount.PhoneNumber);
            if (user == null) return ActiveAccountResult.NotFound;
            if (user.MobileActiveCode == activeAccount.ActiveCode)
            {
                user.MobileActiveCode = new Random().Next(10000, 99999).ToString();
                user.IsMobileActive = true;
                _userRepository.UpdateUser(user);

                await _userRepository.SaveChange();

                return ActiveAccountResult.Success;
            }

            return ActiveAccountResult.Error;
        }

        public async Task<User> GetUserById(long userId)
        {
            return await _userRepository.GetUserById(userId);
        }

        public bool CheckPermission(long permissionId, string phoneNumber)
        {
            return _userRepository.CheckPermission(permissionId, phoneNumber);
        }

        #endregion

        #region Profile

        public async Task<EditUserProfileViewModel> GetEditUserProfile(long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;

            return new EditUserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                UserGender = user.UserGender
            };
        }

        public async Task<EditUserProfileResult> EditProfile(long userId, IFormFile userAvatar, EditUserProfileViewModel editUserProfile)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return EditUserProfileResult.NotFound;

            user.FirstName = editUserProfile.FirstName;
            user.LastName = editUserProfile.LastName;
            user.UserGender = editUserProfile.UserGender;

            if (userAvatar != null && userAvatar.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(userAvatar.FileName);
                userAvatar.AddImageToServer(imageName, PathExtensions.UserAvatarOrginServer, 150, 150,
                    PathExtensions.UserAvatarThumbServer);

                user.Avatar = imageName;
            }

            _userRepository.UpdateUser(user);
            await _userRepository.SaveChange();

            return EditUserProfileResult.Success;
        }

        public async Task<ChangePasswordResult> ChangePassword(long userId, ChangePasswordViewModel changePassword)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                var newPassword = _passwordHelper.EncodePasswordMd5(changePassword.NewPassword);
                if (user.Password != newPassword)
                {
                    user.Password = newPassword;
                    _userRepository.UpdateUser(user);
                    await _userRepository.SaveChange();

                    return ChangePasswordResult.Success;
                }

                return ChangePasswordResult.PasswordEqual;
            }

            return ChangePasswordResult.NotFound;
        }

        public async Task<bool> AddProductToFavorite(long productId, long userId)
        {
            if (!await _userRepository.IsExistProductFavorite(productId, userId))
            {
                //Add
                var newUserFavorite = new UserFavorite
                {
                    ProductId = productId,
                    UserId = userId
                };
                await _userRepository.AddUserFavorite(newUserFavorite);
                await _userRepository.SaveChange();

                return true;
            }

            return false;
        }

        public async Task<bool> AddProductToCompare(long productId, long userId)
        {
            if (!await _userRepository.IsExistProductCompare(productId, userId))
            {
                var newUserCompare = new UserCompare
                {
                    ProductId = productId,
                    UserId = userId
                };
                await _userRepository.AddUserCompare(newUserCompare);
                await _userRepository.SaveChange();

                return true;
            }

            return false;
        }

        public async Task<List<UserCompare>> GetUserCompares(long userId)
        {
            return await _userRepository.GetUserCompares(userId);
        }

        public async Task<int> UserFavoriteCount(long userId)
        {
            return await _userRepository.UserFavoriteCount(userId);
        }

        public async Task<List<UserFavorite>> GetUserFavorites(long userId)
        {
            return await _userRepository.GetUserFavorites(userId);
        }

        public async Task<bool> RemoveAllUserCompare(long userId)
        {
            await _userRepository.RemoveAllRangeUserCompare(userId);
            await _userRepository.SaveChange();

            return true;
        }

        public async Task<bool> RemoveUserCompare(long id)
        {
            await _userRepository.RemoveProductInUserCompare(id);

            await _userRepository.SaveChange();
            return true;
        }

        public async Task<UserComparesViewModel> UserCompares(UserComparesViewModel userCompares)
        {
            return await _userRepository.UserCompares(userCompares);
        }

        public async Task<UserFavoritesViewModel> UserFavorites(UserFavoritesViewModel userFavorites)
        {
            return await _userRepository.UserFavorites(userFavorites);
        }

        #endregion

        #region Admin

        public async Task<FilterUserViewModel> FilterUsers(FilterUserViewModel filter)
        {
            return await _userRepository.FilterUsers(filter);
        }

        public async Task<EditUserFromAdmin> GetEditUserFromAdmin(long userId)
        {
            return await _userRepository.GetEditUserFromAdmin(userId);
        }

        public async Task<EditUserFromAdminResult> EditUserFromAdmin(EditUserFromAdmin editUser)
        {
            var user = await _userRepository.GetUserById(editUser.Id);
            if (user == null)
            {
                return EditUserFromAdminResult.NotFound;
            }

            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.UserGender = editUser.UserGender;

            if (!string.IsNullOrEmpty(editUser.Password))
            {
                user.Password = _passwordHelper.EncodePasswordMd5(editUser.Password);
            }

            _userRepository.UpdateUser(user);

            await _userRepository.RemoveAllPermissionSelectRole(editUser.Id);
            await _userRepository.AddUserToRole(editUser.RoleIds, editUser.Id);

            await _userRepository.SaveChange();


            return EditUserFromAdminResult.Success;
        }

        public async Task<CreateOrEditRoleViewModel> GetEditRoleById(long roleId)
        {
            return await _userRepository.GetEditRoleById(roleId);
        }

        public async Task<CreteOrEditRoleResult> CreateOrEditRole(CreateOrEditRoleViewModel createOrEditRole)
        {
            if (createOrEditRole.Id != 0)
            {

                var role = await _userRepository.GetRoleById(createOrEditRole.Id);
                if (role == null)
                    return CreteOrEditRoleResult.NotFound;

                role.RoleTitle = createOrEditRole.RoleTitle;

                _userRepository.UpdateRole(role);

                await _userRepository.RemoveAllPermissionSelectRole(createOrEditRole.Id);
                if (createOrEditRole.SelectedPermissions == null)
                {
                    return CreteOrEditRoleResult.NotExistPermissions;
                }
                await _userRepository.AddPermissionToRole(createOrEditRole.SelectedPermissions, createOrEditRole.Id);


                await _userRepository.SaveChange();

                return CreteOrEditRoleResult.Success;
                //Edit  
            }
            else
            {
                var newRole = new Role
                {
                    RoleTitle = createOrEditRole.RoleTitle
                };

                await _userRepository.CreateRole(newRole);
                if (createOrEditRole.SelectedPermissions == null)
                {
                    return CreteOrEditRoleResult.NotExistPermissions;
                }
                await _userRepository.AddPermissionToRole(createOrEditRole.SelectedPermissions, newRole.Id);
                await _userRepository.SaveChange();

                return CreteOrEditRoleResult.Success;
                //Create
            }
        }

        public async Task<FilterRolesViewModel> FilterRoles(FilterRolesViewModel filter)
        {
            return await _userRepository.FilterRoles(filter);
        }

        public async Task<List<Permission>> GetAllActivePermission()
        {
            return await _userRepository.GetAllActivePermission();
        }

        public async Task<List<Role>> GetAllActiveRoles()
        {
            return await _userRepository.GetAllActiveRoles();
        }

        #endregion

    }
}