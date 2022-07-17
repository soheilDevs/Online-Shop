using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.ProductEntity;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class FilterProductsViewModel:BasePaging 
    {
        public string ProductName { get; set; }
        public string FilterByCategory  { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductItemViewModel> ProductItemViewModels { get; set; }
        public ProductState ProductState { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public ProductBox ProductBox { get; set; }


        #region Methods

        public FilterProductsViewModel SetProducts(List<Product> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductsViewModel SetProductsItem(List<ProductItemViewModel> productItemView)
        {
            this.ProductItemViewModels = productItemView;
            return this;
        }

        public FilterProductsViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntityCount = paging.AllEntityCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.TakeEntity = paging.TakeEntity;
            this.CountForShowAfterAndBefore = paging.CountForShowAfterAndBefore;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;

            return this;
        }

        #endregion
    }

    public enum ProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        IsActive,
        [Display(Name = "حذف شده")]
        Delete,
    }

    public enum ProductOrder
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "جدید ترین ها")]
        ProductNewses,
        [Display(Name = "گران ترین ها")]
        ProductExpensive,
        [Display(Name = "ارزان ترین ها")]
        ProductInExPensive
    }

    public enum ProductBox
    {
        Default,
        ItemBoxInSite
    }
}