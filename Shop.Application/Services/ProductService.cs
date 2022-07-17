using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Application.Services
{
    public class ProductService : IProductService
    {
        #region Constractor

        private readonly IProcuctRepository _procuctRepository;
        private readonly IUserRepository _userRepository;

        public ProductService(IProcuctRepository procuctRepository, IUserRepository userRepository)
        {
            _procuctRepository = procuctRepository;
            _userRepository = userRepository;
        }

        #endregion

        #region Product-Admin
        #region Product Categories

        public async Task<CreteProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createCategory, IFormFile image)
        {
            if (await _procuctRepository.CheckUrlNameCategories(createCategory.UrlName))
                return CreteProductCategoryResult.IsExistUrlName;

            var newCategory = new ProductCategory
            {
                UrlName = createCategory.UrlName,
                Title = createCategory.Title,
                ParentId = null,
                IsDelete = false
            };
            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtensions.CategoryOrginServer, 150, 150, PathExtensions.CategoryThumbServer);

                newCategory.ImageName = imageName;
            }

            await _procuctRepository.AddProductCategory(newCategory);
            await _procuctRepository.SaveChanges();

            return CreteProductCategoryResult.Success;
        }

        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image)
        {
            var productCategory =
                await _procuctRepository.GetProductCategoryById(editProductCategory.ProductCategoryId);
            if (productCategory == null) return EditProductCategoryResult.NotFound;

            if (await _procuctRepository.CheckUrlNameCategories(editProductCategory.UrlName, editProductCategory.ProductCategoryId))
                return EditProductCategoryResult.IsExistUrlName;
            productCategory.UrlName = editProductCategory.UrlName;
            productCategory.Title = editProductCategory.Title;
            if (image != null && image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                image.AddImageToServer(imageName, PathExtensions.CategoryOrginServer, 150, 150, PathExtensions.CategoryThumbServer, productCategory.ImageName);

                productCategory.ImageName = imageName;
            }
            _procuctRepository.UpdateProductCategory(productCategory);
            await _procuctRepository.SaveChanges();

            return EditProductCategoryResult.Success;
        }

        public async Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filter)
        {
            return await _procuctRepository.FilterProductCategories(filter);
        }


        public async Task<EditProductCategoryViewModel> GetEditProductCategory(long categoryId)
        {
            var productCategory = await _procuctRepository.GetProductCategoryById(categoryId);
            if (productCategory != null)
            {
                return new EditProductCategoryViewModel
                {
                    ImageName = productCategory.ImageName,
                    ProductCategoryId = productCategory.Id,
                    Title = productCategory.Title,
                    UrlName = productCategory.UrlName
                };
            }

            return null;
        }

        #endregion

        #region Product
        public async Task<FilterProductsViewModel> FilterProducts(FilterProductsViewModel filter)
        {
            return await _procuctRepository.FilterProducts(filter);
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct, IFormFile imageProduct)
        {
            #region Product

            var newProduct = new Product
            {
                Name = createProduct.Name,
                Price = createProduct.Price,
                Description = createProduct.Description,
                ShortDescription = createProduct.ShortDescription,
                IsActive = createProduct.IsActive
            };
            if (imageProduct != null && imageProduct.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(imageProduct.FileName);
                imageProduct.AddImageToServer(imageName, PathExtensions.ProductOrginServer, 215, 215,
                    PathExtensions.ProductThumbServer);
                newProduct.ProductImageName = imageName;
            }
            else
            {
                return CreateProductResult.NotImage;
            }

            await _procuctRepository.AddProduct(newProduct);
            await _procuctRepository.SaveChanges();

            await _procuctRepository.AddProductSelectedCategories(createProduct.ProductSelectedCategory, newProduct.Id);

            return CreateProductResult.Success;

            #endregion
        }

        public async Task<List<ProductCategory>> GetAllProductCategories()
        {
            return await _procuctRepository.GetAllProductCategories();
        }

        public async Task<EditProductViewModel> GetEditProduct(long productId)
        {
            var currentProduct = await _procuctRepository.GetProductById(productId);
            if (currentProduct != null)
            {
                return new EditProductViewModel
                {
                    ProductId = currentProduct.Id,
                    Description = currentProduct.Description,
                    ProductImageName = currentProduct.ProductImageName,
                    IsActive = currentProduct.IsActive,
                    Name = currentProduct.Name,
                    Price = currentProduct.Price,
                    ProductSelectedCategory = await _procuctRepository.GetAllProductCategoriesId(productId),
                    ShortDescription = currentProduct.ShortDescription
                };
            }

            return null;

        }

        public async Task<EditProductResult> EditProduct(EditProductViewModel editProduct)
        {
            var product = await _procuctRepository.GetProductById(editProduct.ProductId);

            if (product == null) return EditProductResult.NotFound;

            if (editProduct.ProductSelectedCategory == null) return EditProductResult.ProductSelectedCategoryHasNull;

            #region editProduct

            product.ShortDescription = editProduct.ShortDescription;
            product.Description = editProduct.Description;
            product.IsActive = editProduct.IsActive;
            product.Price = editProduct.Price;
            product.Name = editProduct.Name;
            if (editProduct.ProductImage != null && editProduct.ProductImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(editProduct.ProductImage.FileName);
                editProduct.ProductImage.AddImageToServer(imageName, PathExtensions.ProductOrginServer, 255, 273,
                    PathExtensions.ProductThumbServer, product.ProductImageName);
                product.ProductImageName = imageName;
            }

            #endregion
            _procuctRepository.UpdateProduct(product);

            await _procuctRepository.RemoveProductSelectedCategories(editProduct.ProductId);
            await _procuctRepository.AddProductSelectedCategories(editProduct.ProductSelectedCategory, editProduct.ProductId);
            await _procuctRepository.SaveChanges();
            return EditProductResult.Success;
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            return await _procuctRepository.DeleteProduct(productId);
        }

        public async Task<bool> RecoverProduct(long productId)
        {
            return await _procuctRepository.RecoverProduct(productId);
        }

        public async Task<bool> AddProductGallery(long productId, List<IFormFile> images)
        {
            if (!await _procuctRepository.CheckProduct(productId))
            {
                return false;
            }

            if (images != null && images.Any())
            {
                var productGallery = new List<ProductGalleries>();
                foreach (var image in images)
                {
                    if (image.IsImage())
                    {
                        var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(image.FileName);
                        image.AddImageToServer(imageName, PathExtensions.ProductOrginServer, 255, 273,
                            PathExtensions.ProductThumbServer);

                        productGallery.Add(new ProductGalleries
                        {
                            ImageName = imageName,
                            ProductId = productId
                        });
                    }
                }

                await _procuctRepository.AddProductGalleries(productGallery);

            }
            return true;
        }

        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _procuctRepository.GetAllProductGalleries(productId);
        }

        public async Task DeleteImage(long galleryId)
        {
            var productGallery = await _procuctRepository.GetProductGalleriesById(galleryId);
            if (productGallery!=null)
            {
                UploadImageExtension.DeleteImage(productGallery.ImageName,PathExtensions.ProductOrginServer,PathExtensions.ProductThumbServer);
                await _procuctRepository.DeleteProductGallery(galleryId);
            }
        }

        public async Task<CreateProductFeaturesResult> CreateProductFeatures(CreateProductFeaturesViewModel createProductFeatures)
        {
            if (!await _procuctRepository.CheckProduct(createProductFeatures.ProductId))
            {
                return CreateProductFeaturesResult.Error;
            }
            var newProductFeatures = new ProductFeature
            {
                FeatureTitle = createProductFeatures.Title,
                FeatureValue = createProductFeatures.Value,
                ProductId = createProductFeatures.ProductId
            };

            await _procuctRepository.AddProDuctFeatures(newProductFeatures);
            await _procuctRepository.SaveChanges();

            return CreateProductFeaturesResult.Success;
        }

        public async Task<List<ProductFeature>> GetProductFeature(long productId)
        {
            return await _procuctRepository.GetProductFeature(productId);
        }

        public async Task DeleteFeatures(long id)
        {
            await _procuctRepository.DeleteFeatures(id);
        }

        public async Task<List<ProductItemViewModel>> ShowAllProductInSlider()
        {
            return await _procuctRepository.ShowAllProductInSlider();
        }

        public async Task<List<ProductItemViewModel>> ShowAllProductInCategory(string hrefName)
        {
            return await _procuctRepository.ShowAllProductInCategory(hrefName);
        }

        public async Task<List<ProductItemViewModel>> LastProduct()
        {
            return await _procuctRepository.LastProduct();
        }

        public async Task<ProductDetailViewModel> ShowProductDetail(long productId)
        {
            return await _procuctRepository.ShowProductDetail(productId);
        }

        public async Task<CreateProductCommentResult> CreateProductComment(CreateProductCommentViewModel createProduct, long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user==null)
            {
                return CreateProductCommentResult.CheckUser;    
            }

            if (!await _procuctRepository.CheckProduct(createProduct.ProductId))
            {
                return CreateProductCommentResult.CheckProduct;
            }

            var newComment = new ProductComment
            {
                ProductId = createProduct.ProductId,
                UserId = userId,
                Text = createProduct.Text
            };

            await _procuctRepository.AddProductComment(newComment);
            await _procuctRepository.SaveChanges();

            return CreateProductCommentResult.Success;
        }

        public async Task<List<ProductComment>> AllProductCommentById(long productId)
        {
            return await _procuctRepository.AllProductCommentById(productId);
        }

        public async Task<List<ProductItemViewModel>> GetRElatedProduct(string categoryName,long productId)
        {
            return await _procuctRepository.GetRElatedProduct(categoryName,productId);
        }

        #endregion
        #endregion


    }
}