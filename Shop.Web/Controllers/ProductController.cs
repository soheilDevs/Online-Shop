using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;
using Shop.Web.Extentions;

namespace Shop.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Constractor

        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public ProductController(IProductService productService, IOrderService orderService)
        {
            _productService = productService;
            _orderService = orderService;
        }

        #endregion

        #region Products
        [HttpGet("products")]
        public async Task<IActionResult> Products(FilterProductsViewModel filter)
        {
            filter.TakeEntity = 12;
            filter.ProductBox = ProductBox.ItemBoxInSite;

            ViewData["Categories"] = await _productService.GetAllProductCategories();
            return View(await _productService.FilterProducts(filter));
        }
        #endregion

        #region Show-Product
        [HttpGet("showDetail/{productId}")]
        public async Task<IActionResult> ProductDetail(long productId)
        {
            var data = await _productService.ShowProductDetail(productId);
            if (data == null)
            {
                return NotFound();
            }

            TempData["relatedProduct"] = await _productService.GetRElatedProduct(data.ProductCategory.UrlName,productId);
            return View(data);
        }

        #endregion

        #region Create-Product-Comment

        [HttpPost("add-comment"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductComment(CreateProductCommentViewModel createProductComment)
        {
            if (!string.IsNullOrEmpty(createProductComment.Text))
            {
                var result = await _productService.CreateProductComment(createProductComment, User.GetUserId());
                switch (result)
                {
                    case CreateProductCommentResult.CheckUser:
                        TempData[WarningMessage] = "کاربر مورد نظر یاقت نشد";
                        break;
                    case CreateProductCommentResult.CheckProduct:
                        TempData[WarningMessage] = "محصولی یاقت نشد";
                        break;
                    case CreateProductCommentResult.Success:
                        TempData[SuccessMessage] = "نظر شما با موفقیت ثبت شد";
                        return RedirectToAction("ProductDetail", new { productId = createProductComment.ProductId });

                }
            }

            TempData[ErrorMessage] = "لطفا نظر خود را وارد نمایید";
            return RedirectToAction("ProductDetail", new { productId = createProductComment.ProductId });

        }


        #endregion

        #region Buy-Product
        [Authorize]
        public async Task<IActionResult> BuyProduct(long productId)
        {
            long orderId = await _orderService.AddOrder(User.GetUserId(), productId);
            return Redirect("/User/Basket/" + orderId);
        }

        #endregion
    }
}
