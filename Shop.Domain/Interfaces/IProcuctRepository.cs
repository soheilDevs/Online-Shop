using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Domain.Interfaces
{
    public interface IProcuctRepository
    {
        Task SaveChanges();

        #region Product Categories

        Task<bool> CheckUrlNameCategories(string urlName);
        Task<bool> CheckUrlNameCategories(string urlName,long categoryId);
        Task AddProductCategory(ProductCategory productCategory);
        Task<ProductCategory> GetProductCategoryById(long id);
        void UpdateProductCategory(ProductCategory category);
        Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filter);
        Task<List<ProductCategory>> GetAllProductCategories();
       
        #endregion

        #region Product

        Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter);
        Task AddProduct(Product product);
        Task RemoveProductSelectedCategories(long productId);
        Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId);
        Task<Product> GetProductById(long productId);
        Task<List<long>> GetAllProductCategoriesId(long productId);
        void UpdateProduct(Product product);

        Task<bool> DeleteProduct(long productId);
        Task<bool> RecoverProduct(long productId);

        Task AddProductGalleries(List<ProductGalleries> productGalleries);
        Task<bool> CheckProduct(long productId);

        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task<ProductGalleries> GetProductGalleriesById(long id);
        Task DeleteProductGallery(long id);

        Task AddProDuctFeatures(ProductFeature feature);
        Task<List<ProductFeature>> GetProductFeature(long productId);
        Task DeleteFeatures(long id);

        Task<List<ProductItemViewModel>> ShowAllProductInSlider();

        Task<List<ProductItemViewModel>> ShowAllProductInCategory(string hrefName);

        Task<List<ProductItemViewModel>> LastProduct();


        Task<ProductDetailViewModel> ShowProductDetail(long productId);

        Task AddProductComment(ProductComment comment);
        Task<List<ProductComment>> AllProductCommentById(long productId);

        Task<List<ProductItemViewModel>> GetRElatedProduct(string categoryName,long productId);


        #endregion

    }
}