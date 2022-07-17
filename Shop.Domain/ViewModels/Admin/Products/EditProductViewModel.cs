using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class EditProductViewModel
    {
        public long ProductId { get; set; }
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string Name { get; set; }
        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(800, ErrorMessage = "نمیتواند از {1} کاراکتر بیشتر باشد ")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }
        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }
        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }

        public string ProductImageName { get; set; }
        public List<long> ProductSelectedCategory { get; set; }
        public IFormFile ProductImage { get; set; }                // ino mishe to servicam gereft mesle edit ProductCategory
    }

    public enum EditProductResult
    {
        NotFound,
        ProductSelectedCategoryHasNull,
        Success
    }
}