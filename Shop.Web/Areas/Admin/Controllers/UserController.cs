using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Account;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        #region Constractor
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region FilterUser

        public async Task<IActionResult> FilterUser(FilterUserViewModel filter)
        {
            var data = await _userService.FilterUsers(filter);


            return View(data);
        }

        #endregion

        #region editUser
        [HttpGet]
        public async Task<IActionResult> EditUser(long userId)
        {
            var data = await _userService.GetEditUserFromAdmin(userId);

            if (data == null)
            {
                return NotFound();
            }

            ViewData["Roles"] = await _userService.GetAllActiveRoles();
            return View(data);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserFromAdmin editUser)
        {
            ViewData["Roles"] = await _userService.GetAllActiveRoles();

            if (ModelState.IsValid)
            {
                var result = await _userService.EditUserFromAdmin(editUser);

                switch (result)
                {
                    case EditUserFromAdminResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserFromAdminResult.Success:
                        TempData[SuccessMessage] = "ویرایش کاربر با موفقیت انجام شد";
                        return RedirectToAction("FilterUser");
                }
            }

            return View(editUser);
        }
        #endregion

        #region FilterRoles

        public async Task<IActionResult> FilterRoles(FilterRolesViewModel filter)
        {
            return View(await _userService.FilterRoles(filter));
        }

        #endregion

        #region CreateRole
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateOrEditRoleViewModel create)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateOrEditRole(create);
                switch (result)
                {
                    case CreteOrEditRoleResult.NotFound:
                        break;
                    case CreteOrEditRoleResult.NotExistPermissions:
                        TempData[WarningMessage] = "لطفا نقشی را لنتخاب کنید";
                        break;
                    case CreteOrEditRoleResult.Success:
                        TempData[SuccessMessage] = "عملیات افزودن نقش با موفقیت انجام شد ";
                        return RedirectToAction("FilterRoles");
                }
            }
            return View();
        }
        #endregion

        #region EditRole
        [HttpGet]
        public async Task<IActionResult> EditRole(long roleId)
        {
            ViewData["Permissions"] = await _userService.GetAllActivePermission();
            return View(await _userService.GetEditRoleById(roleId));
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(CreateOrEditRoleViewModel create)
        {
            ViewData["Permissions"] = await _userService.GetAllActivePermission();
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateOrEditRole(create);
                switch (result)
                {
                    case CreteOrEditRoleResult.NotFound:
                        TempData[WarningMessage] = "نفشی با مشخصات وارد شده یافت نشد";
                        break;
                    case CreteOrEditRoleResult.NotExistPermissions:
                        TempData[WarningMessage] = "لطفا نقشی را لنتخاب کنید";
                        break;
                    case CreteOrEditRoleResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش نقش با موفقیت انجام شد ";
                        return RedirectToAction("FilterRoles");
                }
            }
            return View();
        }
        #endregion
    }
}
