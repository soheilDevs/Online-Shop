using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Admin.Order;
using Shop.Domain.ViewModels.Wallet;
using Shop.Web.Extentions;
using ZarinpalSandbox;

namespace Shop.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region Constractor

        private readonly IUserService _userService;
        private readonly IWalletService _walletService;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;

        public AccountController(IUserService userService, IWalletService walletService, IConfiguration configuration, IOrderService orderService)
        {
            _userService = userService;
            _walletService = walletService;
            _configuration = configuration;
            _orderService = orderService;
        }

        #endregion

        #region EditUserProfile
        [HttpGet("edit-user-profile")]
        public async Task<IActionResult> EditUserProfile()
        {
            var user = await _userService.GetEditUserProfile(User.GetUserId());
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost("edit-user-profile"),ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfile(EditUserProfileViewModel editUserProfile,IFormFile userAvatar)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.EditProfile(User.GetUserId(), userAvatar, editUserProfile);
                switch (result)
                {
                    case EditUserProfileResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش خساب کاربری با موفقیت انجام شد";
                        return  RedirectToAction("EditUserProfile");
                  
                }
            }
            return View(editUserProfile);
        }

        #endregion

        #region ChangePassword
        [HttpGet("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassword) 
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePassword(User.GetUserId(), changePassword);
                switch (result)
                {
                    case ChangePasswordResult.NotFound:
                        TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                        break;
                    case ChangePasswordResult.PasswordEqual:
                        TempData[InfoMessage] = "لطفا از کلمه عبور جدیدی استفاده کنید";
                        ModelState.AddModelError("NewPassword", "لطفا از کلمه عبور جدیدی استفاده کنید");
                        break;
                    case ChangePasswordResult.Success:
                        TempData[SuccessMessage] = "کلمه عبور شما یا موفقین تغییر یافت";
                        TempData[InfoMessage] = "لطفا جهت تکمیل فراید تغیر کلمه ی عبور ،مجددا وارد سایت شوید";
                        await HttpContext.SignOutAsync();
                        return RedirectToAction("Login", "Account", new {area = ""});
                }
            }
            return View(changePassword);
        }

        #endregion

        #region ChargeWallet
        [HttpGet("charge-wallet")]
        public async Task<IActionResult> ChargeWallet()
        {
            return View();
        }

        [HttpPost("charge-wallet"),ValidateAntiForgeryToken]
        public async Task<IActionResult> ChargeWallet(ChargeWalletViewModel chargeWallet)
        {
            if (ModelState.IsValid)
            {
                var walletId = await _walletService.ChargeWallet(User.GetUserId(), chargeWallet,
                    $"شارژ به مبلغ {chargeWallet.Amount}");

                #region Payment

                var payment = new Payment(chargeWallet.Amount);
                var url = _configuration.GetSection("DefaultUrl")["Host"] + "/user/online-payment/" + walletId;
                var result = payment.PaymentRequest("شارژ کیف پول", url);

                if (result.Result.Status==100)   //100 yani hamechi ok e
                {
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
                }
                else
                {
                    TempData[ErrorMessage] = "مشکلی در پرداخت به وجود آماده است،لطفا مجددا امتحان کنید";
                }

                #endregion
            }
            return View(chargeWallet);
        }

        #endregion

        #region OnlinePayment
        [HttpGet("online-payment/{id}")]
        public async Task<IActionResult> OnlinePayment(long id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var wallet = await _walletService.GetUserWalletById(id);
                if (wallet != null)
                {
                    var payment = new Payment(wallet.Amount);
                    var result = payment.Verification(authority).Result;

                    if (result.Status == 100)
                    {
                        ViewBag.RefId = result.RefId;
                        ViewBag.Success = true;
                        await _walletService.UpdateWalletForCharge(wallet);
                    }

                    return View();
                }

                return NotFound();
            }
            return View();
        }

        #endregion

        #region UserWallet
        [HttpGet("user-wallet")]
        public async Task<IActionResult> UserWallet(FilterWalletViewModel filter)
        {
            filter.UserId = User.GetUserId();
            filter.TakeEntity = 1;
            //filter.CountForShowAfterAndBefore = 4;
            return View(await _walletService.FilterWallets(filter));
        }

        #endregion

        #region User-Basket
        [HttpGet("basket/{orderId}")]
        public async Task<IActionResult> UserBasket(long orderId)
        {
            var order = await _orderService.GetUserBasket(orderId, User.GetUserId());
            if (order==null)
            {
                return NotFound();  
            }

            ViewBag.UserWalletAmount = await _walletService.GetUserWalletAmount(User.GetUserId());

            return View(order);
        }

        [HttpPost("basket/{orderId}"),ValidateAntiForgeryToken]
        public async Task<IActionResult> UserBasket(FinallyOrderViewModel finallyOrder)
        {
            if (finallyOrder.IsWallet)
            {
                var result = await _orderService.FinallyOrder(finallyOrder, User.GetUserId());

                switch (result)
                {
                    case FinallyOrderResult.HasNotUser:
                        TempData[ErrorMessage] = "سفارش شما یافت نشد";
                        break;
                    case FinallyOrderResult.NotFound:
                        TempData[ErrorMessage] = "سفارش شما یافت نشد";
                        break;
                    case FinallyOrderResult.Error:
                        TempData[ErrorMessage] = "موجودی حساب شما کافی نمیباشد";
                        return RedirectToAction("UserWallet");
                    case FinallyOrderResult.Success:
                        TempData[SuccessMessage] = "فاکتور شما با موفقیت پرداخت شد از خرید شما متشکریم";
                        return RedirectToAction("UserWallet");

                }
            }
            else
            {
                var order = await _orderService.GetOrderById(finallyOrder.OrderId);
                #region Payment

                var payment = new Payment(order.OrderSum);
                var url = _configuration.GetSection("DefaultUrl")["Host"] + "/user/online-order/" + order.Id;
                var result = payment.PaymentRequest("شارژ کیف پول", url);

                if (result.Result.Status == 100)   //100 yani hamechi ok e
                {
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Result.Authority);
                }
                else
                {
                    TempData[ErrorMessage] = "مشکلی در پرداخت به وجود آماده است،لطفا مجددا امتحان کنید";
                }

                #endregion
            }



            ViewBag.UserWalletAmount = await _walletService.GetUserWalletAmount(User.GetUserId());

            return View();
        }

        #endregion

        #region Delete-OrderDetail
        [HttpGet("delete-order-detail/{orderDetailId}")]
        public async Task<IActionResult> DeleteOrderDetail(long orderDetailId)
        {
            var result = await _orderService.RemoveOrderDetailFromOrder(orderDetailId);

            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region Reload Price

        [HttpGet("reload-price")]
        public async Task<IActionResult>ReloadOrderPrice(long id)
        {
            var order = await _orderService.GetUserBasket(id, User.GetUserId());
            ViewBag.UserWalletAmount = await _walletService.GetUserWalletAmount(User.GetUserId());

            return PartialView("_OrderPrice", order);
        }

        #endregion

        #region Order Payment
        [HttpGet("online-order/{id}")]
        public async Task<IActionResult> OrderPayment(long id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];
                var order = await _orderService.GetOrderById(id);
                if (order != null)
                {
                    var payment = new Payment(order.OrderSum);
                    var result = payment.Verification(authority).Result;

                    if (result.Status == 100)
                    {
                        ViewBag.RefId = result.RefId;
                        ViewBag.Success = true;
                        await _orderService.ChangeIsFinallyToOrder(order.Id);
                    }

                    return View();
                }

                return NotFound();
            }
            return View();
        }

        #endregion

        #region User-Orders
        [HttpGet("user-orders")]
        public async Task<IActionResult> UserOrders(FilterOrdersViewModel filter)
        {
            filter.UserId = User.GetUserId();


            return View(await _orderService.FilterOrders(filter));
        }

        #endregion

        #region User Favorite
        [HttpGet("add-favorite/{productId}")]
        public async Task<IActionResult> AddUserFavorite(long productId)
        {
            var result = await _userService.AddProductToFavorite(productId, User.GetUserId());
            if (result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر با موفقیت در قسمت علاقه مندی اضافه شد";
                return RedirectToAction("UserFavorites");
            }

            TempData[WarningMessage] = "محصول مورد نظر قبلا در علاقه مندی اضافه شده است";
            return RedirectToAction("UserFavorites");
        }

        #endregion

        #region User Compare
        [HttpGet("add-compare/{productId}")]
        public async Task<IActionResult> AddUserCompare(long productId)
        {
            var result = await _userService.AddProductToCompare(productId, User.GetUserId());
            if (result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر با موفقیت در قسمت مقایسه اضافه شد";
                return RedirectToAction("UserCompares");
            }

            TempData[WarningMessage] = "محصول مورد نظر قبلا در مقایسه اضافه شده است";
            return RedirectToAction("UserCompares");
        }

        #endregion

        #region RemoveAllUserCompare
        [HttpGet("removeAllUserCompare")]

        public async Task<IActionResult> RemoveAllUserCompare()
        {
            var result = await _userService.RemoveAllUserCompare(User.GetUserId());
            if (result)
            {
                TempData[SuccessMessage] = "تمامی محصولاتی که در لیست مقایسه بود حذف شد";
                return RedirectToAction("UserCompares");
            }

            TempData[WarningMessage] = "لیست مقایسه شما خالی میباشد";
            return RedirectToAction("UserCompares");
        }

        #endregion

        #region RemoveUserCompare
        [HttpGet("RemoveUserCompare")]
        public async Task<IActionResult> RemoveUserCompare(long id)
        {
            var result = await _userService.RemoveUserCompare(id);
            if (result)
            {
                TempData[SuccessMessage] = "محصول مورد نظر که در لیست مقایسه بود حذف شد";
                return RedirectToAction("UserCompares");
            }

            TempData[WarningMessage] = "همچین محصولی در لیست مقایسه شما وجود ندارد";
            return RedirectToAction("UserCompares");
        }

        #endregion

        #region ListUserFavorites
        [HttpGet("user-favorites")]
        public async Task<IActionResult> UserFavorites(UserFavoritesViewModel filter)
        {
            return View(await _userService.UserFavorites(filter));
        }

        #endregion

        #region ListUserCompares
        [HttpGet("user-compares")]
        public async Task<IActionResult> UserCompares(UserComparesViewModel filter)
        {
            return View(await _userService.UserCompares(filter));
        }

        #endregion
    }
}
