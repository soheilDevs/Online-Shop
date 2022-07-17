﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class CreateProductViewModel
    {
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

        public List<long> ProductSelectedCategory { get; set; }
    }

    public enum CreateProductResult
    {
        NotImage,
        Success
    }
}