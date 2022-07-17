using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Sliders;
using Shop.Web.Extentions;

namespace Shop.Web.ViewComponents
{
    #region Site Header
    public class SiteHeaderViewComponent:ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;

        public SiteHeaderViewComponent(IUserService userService, IOrderService orderService)
        {
            _userService = userService;
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _userService.GetUserByPhoneNumber(User.Identity.Name);
                ViewBag.Order = await _orderService.GetUserBasket(User.GetUserId());
                ViewBag.UserCompare = await _userService.GetUserCompares(User.GetUserId());
                ViewBag.FavoriteCount = await _userService.UserFavoriteCount(User.GetUserId());
            }
            return View("SiteHeader");
        }
    }
    #endregion

    #region Site Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }
    }
    #endregion

    #region Slider-Home 
    public class SliderHomeViewComponent : ViewComponent
    {
        #region Constractor

        private readonly ISiteSettingService _siteSettingService;

        public SliderHomeViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filterSlidersViewModel = new FilterSlidersViewModel()
            {
                TakeEntity = 10
            };
            var data = await _siteSettingService.FilterSliders(filterSlidersViewModel);
            return View("SliderHome",data);
        }
    }
    #endregion

    #region Popular Category-Home 
    public class PopularCategoryViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public PopularCategoryViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filterCategory = new FilterProductCategoriesViewModel()
            {
                TakeEntity = 6
            };
            var data = await _productService.FilterProductCategories(filterCategory);
            return View("PopularCategory", data);
        }
    }
    #endregion

    #region Popular Category-Home 
    public class SideBarCategoryViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public SideBarCategoryViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filterCategory = new FilterProductCategoriesViewModel();
            var data = await _productService.FilterProductCategories(filterCategory);
            return View("SideBarCategory", data);
        }
    }
    #endregion

    #region All-ProductInSlider-Home 
    public class AllProductInSliderViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public AllProductInSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInSlider();
            return View("AllProductInSlider", data);
        }
    }
    #endregion

    #region All-ProductInCategoryPc-Home 
    public class AllInCategoryPcViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public AllInCategoryPcViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("pc");
            return View("AllInCategoryPc", data);
        }
    }
    #endregion

    #region All-ProductInCategoryTv-Home 
    public class AllInCategoryTvViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public AllInCategoryTvViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("tv");
            return View("AllInCategoryTv", data);
        }
    }
    #endregion

    #region All-ProductInCategoryPhone-Home 
    public class AllInCategoryPhoneViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public AllInCategoryPhoneViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("Mobile");
            return View("AllInCategoryPhone", data);
        }
    }
    #endregion

    #region All-ProductInCategoryWatch-Home 
    public class AllInCategoryWatchViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public AllInCategoryWatchViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("smartWatch");
            return View("AllInCategoryWatch", data);
        }
    }
    #endregion

    #region All-ProductInCategoryAccessory-Home 
    public class AllInCategoryAccessoryViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public AllInCategoryAccessoryViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = await _productService.ShowAllProductInCategory("accessory");
            return View("AllInCategoryAccessory", data);
        }
    }
    #endregion

    #region All-ProductInCategoryAccessory-Home 
    public class ProductCommentsViewComponent : ViewComponent
    {
        #region Constractor

        private readonly IProductService _productService;

        public ProductCommentsViewComponent   (IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync(long productId)
        {
            var data = await _productService.AllProductCommentById(productId);
            return View("ProductComments", data);
        }
    }
    #endregion

}
