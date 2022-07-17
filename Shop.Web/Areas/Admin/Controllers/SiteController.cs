using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.Application.Interfaces;
using Shop.Domain.ViewModels.Site.Sliders;

namespace Shop.Web.Areas.Admin.Controllers
{
    public class SiteController : AdminBaseController
    {
        #region Constractor

        private readonly ISiteSettingService _siteSettingService;

        public SiteController(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        #endregion

        #region Sliders

        public async Task<IActionResult> FilterSlider(FilterSlidersViewModel filter)
        {
            return View(await _siteSettingService.FilterSliders(filter));
        }

        #endregion

        #region Create-Slider
        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(CreateSliderViewModel createSlider)
        {
            if (ModelState.IsValid)
            {
                var result = await _siteSettingService.CreateSlider(createSlider);

                switch (result)
                {
                    case CreateSliderResult.ImageNotFond:
                        TempData[WarningMessage] = "";
                        break;
                    case CreateSliderResult.Success:
                        TempData[SuccessMessage] = "";
                        return RedirectToAction("FilterSlider");
                   
                }
            }

            return View(createSlider);
        }
        #endregion

        #region Edit-Slider
        [HttpGet]
        public async Task<IActionResult> EditSlider(long sliderId)
        {
            var data = await _siteSettingService.GetEditSlider(sliderId);
            if (data==null)
            {
                NotFound(); 
            }
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlider(EditSliderViewModel editSlider)
        {
            if (ModelState.IsValid)
            {
                var result = await _siteSettingService.EditSlider(editSlider);
                switch (result)
                {
                    case EditSliderResult.NotFound:
                        TempData[WarningMessage] = "";
                        break;
                    case EditSliderResult.Success:
                        TempData[SuccessMessage] = "";
                        return RedirectToAction("FilterSlider");

                }
            }

            return View(editSlider);
        }

        #endregion
    }
}
