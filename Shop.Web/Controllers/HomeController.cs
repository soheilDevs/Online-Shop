using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Interfaces;

namespace Shop.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region Constractor

        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IActionResult> Index()
        {
            ViewData["LastProducts"] = await _productService.LastProduct();
            return View();
        }

        public IActionResult Errors()
        {
            return View();
        }

       
    }
}
