using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Application.Interfaces
{
    public interface IProductService
    {
        #region Product-Admin

        Task<CreteProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createCategory,IFormFile image);
        Task<EditProductCategoryViewModel> GetEditProductCategory(long categoryId);
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory,IFormFile image);
        Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filter);
        Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter);
        Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct,IFormFile imageProduct);
        Task<List<ProductCategory>> GetAllProductCategories();
        Task<EditProductViewModel> GetEditProduct(long productId);
        Task<EditProductResult> EditProduct(EditProductViewModel editProduct);

        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);

        Task<bool> AddProductGallery(long productId, List<IFormFile> images);

        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task DeleteImage(long galleryId);

        Task<CreateProductFeaturesResult> CreateProductFeatures(CreateProductFeaturesViewModel createProductFeatures);
        Task<List<ProductFeature>> GetProductFeature(long productId);
        Task DeleteFeatures(long id);

        Task<List<ProductItemViewModel>> ShowAllProductInSlider();
        Task<List<ProductItemViewModel>> ShowAllProductInCategory(string hrefName);
        Task<List<ProductItemViewModel>> LastProduct();

        Task<ProductDetailViewModel> ShowProductDetail(long productId);

        Task<CreateProductCommentResult> CreateProductComment(CreateProductCommentViewModel createProduct, long userId);
        Task<List<ProductComment>> AllProductCommentById(long productId);

        Task<List<ProductItemViewModel>> GetRElatedProduct(string categoryName, long productId);
        #endregion
    }
}