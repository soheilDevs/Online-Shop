using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.ProductEntity
{
    public class ProductSelectedCategories:BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }
        public long ProductCategoryId { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }

        #endregion
    }
}