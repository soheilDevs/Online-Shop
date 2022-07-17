using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Interfaces;
using Shop.Web.Permission;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Constractor

        private readonly IOrderService _orderService;

        public HomeController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #endregion


        #region Dashbord
        //[PermissionChecker(1)]
        public async Task<IActionResult> Index()
        {
            ViewData["ResultOrder"] = await _orderService.GetResultOrder();
            return View();
        }

        #endregion
    }
}
