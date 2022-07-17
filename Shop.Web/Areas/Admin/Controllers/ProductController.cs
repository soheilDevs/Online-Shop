using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Web.Extentions;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Constractor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion


        #region Products

        #region Filter - Products 

        public async Task<IActionResult> FilterProducts(FilterProductsViewModel filter)
        {
            var data = await _productService.FilterProducts(filter);
            return View(data);
        }

        #endregion

        #region Create-Product
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            TempData["Categories"] = await _productService.GetAllProductCategories();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createProduct, IFormFile productImage)
        {
            //TempData["Categories"] = await _productService.GetAllProductCategories();
            if (ModelState.IsValid)
            {

                var result = await _productService.CreateProduct(createProduct, productImage);

                switch (result)
                {
                    case CreateProductResult.NotImage:
                        TempData[WarningMessage] = "لطفا برای محصول یک تصویر انتخاب کنید";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = "عملیات ثبت محصول با موفقیت انجام شد";
                        return RedirectToAction("FilterProducts");

                }
            }

            return View(createProduct);
        }
        #endregion

        #region Edit-Product
        [HttpGet]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var data = await _productService.GetEditProduct(productId);
            if (data == null)
            {
                return NotFound();
            }
            TempData["Categories"] = await _productService.GetAllProductCategories();
            return View(data);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductViewModel editProduct)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProduct(editProduct);
                switch (result)
                {
                    case EditProductResult.NotFound:
                        TempData[WarningMessage] = "محصولی با مشخصات وارد شده یافت نشد";
                        break;
                    case EditProductResult.ProductSelectedCategoryHasNull:
                        TempData[WarningMessage] = "لطفا دسته بندی محصول را وارد کنید";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "ویرایش محصول با موفقیت انجام شد";
                        return RedirectToAction("FilterProducts");
                }
            }

            return View(editProduct);
        }
        #endregion

        #region Delete-Product

        public async Task<IActionResult> DeleteProduct(long productId)
        {
            var result = await _productService.DeleteProduct(productId);
            if (result)
            {
                TempData[SuccessMessage] = "محصول شما با موفقیت حذف شد";
                return RedirectToAction("FilterProducts");
            }

            TempData[WarningMessage] = "در حذف محصول خطایی رخ داده است";
            return RedirectToAction("FilterProducts");
        }

        #endregion

        #region Recover-Product

        public async Task<IActionResult> RecoverProduct(long productId)
        {
            var result = await _productService.RecoverProduct(productId);
            if (result)
            {
                TempData[SuccessMessage] = "محصول شما با موفقیت بازگردانی شد";
                return RedirectToAction("FilterProducts");
            }

            TempData[WarningMessage] = "در بازگردانی محصول خطایی رخ داده است";
            return RedirectToAction("FilterProducts");
        }

        #endregion
        #endregion

        #region Category

        #region Filter-Products-Categories

        public async Task<IActionResult> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            return View(await _productService.FilterProductCategories(filter));
        }
        #endregion

        #region Create Product Category
        [HttpGet]
        public IActionResult CreateProductCategory()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryViewModel createProductCategory,
            IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductCategory(createProductCategory, image);
                switch (result)
                {
                    case CreteProductCategoryResult.IsExistUrlName:
                        TempData[WarningMessage] = "اسمUrl تکراری است";
                        break;
                    case CreteProductCategoryResult.Success:
                        TempData[SuccessMessage] = "دسته بندی با موفقیت ثبت شد";
                        return RedirectToAction("FilterProductCategories");
                }
            }

            return View(createProductCategory);
        }
        #endregion

        #region EditProduct Category
        [HttpGet]
        public async Task<IActionResult> EditProductCategory(long categoryId)
        {
            var data = await _productService.GetEditProductCategory(categoryId);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(EditProductCategoryViewModel editProductCategory,
            IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProductCategory(editProductCategory, image);
                switch (result)
                {
                    case EditProductCategoryResult.IsExistUrlName:
                        TempData[WarningMessage] = "اسمUrl تکراری است";
                        break;
                    case EditProductCategoryResult.NotFound:
                        TempData[ErrorMessage] = "دسته بندی با مشخصات وارد شده یافت نشد";
                        break;
                    case EditProductCategoryResult.Success:
                        TempData[SuccessMessage] = "دسته بندی با موفقیت ویرایش شد";
                        return RedirectToAction("FilterProductCategories");
                }
            }

            return View(editProductCategory);
        }

        #endregion

        #endregion

        #region Product Galleries

        #region Create

        public IActionResult GalleryProduct(long productId)
        {
            ViewBag.productId = productId;
            return View();
        }

        public async Task<IActionResult> AddImageToProduct(List<IFormFile> images, long productId) //faghat baraye add kardan 
        {
            var result = await _productService.AddProductGallery(productId, images);
            if (result)
            {
                return JsonResponseStatus.Success();
            }

            return JsonResponseStatus.Error();
        }

        #endregion

        #region List Prodyct Gallary

        public async Task<IActionResult> ProductGalleries(long productId)
        {
            var data = await _productService.GetAllProductGalleries(productId);
            return View(data);
        }

        #endregion

        #region Delete Image

        public async Task<IActionResult> DeleteImage(long galleryId)
        {
            await _productService.DeleteImage(galleryId);
            return RedirectToAction("FilterProducts");
        }


        #endregion

        #endregion

        #region Product Features

        #region Create

        [HttpGet]
        public IActionResult CreateProductFeatures(long productId)
        {
            var model = new CreateProductFeaturesViewModel     //beja mishe az viewbag ham estefade kard mesle qablia in raveshe digast
            {
                ProductId = productId
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductFeatures(CreateProductFeaturesViewModel featuresViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductFeatures(featuresViewModel);

                switch (result)
                {
                    case CreateProductFeaturesResult.Error:
                        TempData[ErrorMessage] = "در ثبت ویژگی خطایی زخ داده است";
                        break;
                    case CreateProductFeaturesResult.Success:
                        TempData[SuccessMessage] = "ویژگی با موفقیت ثبت شد";
                        return RedirectToAction("FilterProducts");
                    
                }
            }

            return View(featuresViewModel);
        }

        #endregion

        #region Product Features

        public async Task<IActionResult> ProductFeatures(long productId)
        {
            return View(await _productService.GetProductFeature(productId));
        }

        #endregion

        #region Delete Product Features

        public async Task<IActionResult> DeleteFeatures(long featureId)
        {
            await _productService.DeleteFeatures(featureId);
            return RedirectToAction("FilterProducts");
        }

        #endregion
        #endregion

    }
}
