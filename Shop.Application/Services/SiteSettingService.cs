using System;
using System.IO;
using System.Threading.Tasks;
using Shop.Application.Extentions;
using Shop.Application.Interfaces;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Site.Sliders;

namespace Shop.Application.Services
{
    public class SiteSettingService:ISiteSettingService
    {
        #region Constractor

        private ISiteSettingRepository _siteSettingRepository;

        public SiteSettingService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        #endregion

        #region Slider

        public async Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filterSlidersViewModel)
        {
           return await _siteSettingRepository.FilterSliders(filterSlidersViewModel);
        }

        public async Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider)
        {
            var newSlider = new Slider
            {
                Href = createSlider.Href,
                Price = createSlider.Price,
                SliderText = createSlider.SliderText,
                SliderTitle = createSlider.SliderTitle,
                TextBtn = createSlider.TextBtn
            };

            if (createSlider.ImageFile!=null &&createSlider.ImageFile.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(createSlider.ImageFile.FileName);
                createSlider.ImageFile.AddImageToServer(imageName, PathExtensions.SliderOrginServer, 255, 273,
                    PathExtensions.SliderThumbServer);
                newSlider.SliderImage = imageName;    
            }
            else
            {
                return CreateSliderResult.ImageNotFond;
            }

            await _siteSettingRepository.AddSlider(newSlider);
            await _siteSettingRepository.SaveChanges();

            return CreateSliderResult.Success;
        }

        public async Task<EditSliderViewModel> GetEditSlider(long sliderId)
        {
            var currentSlider = await _siteSettingRepository.GetSliderById(sliderId);
            if (currentSlider==null)
            {
                return null;
            }

            return new EditSliderViewModel
            {
                Href = currentSlider.Href,
                SliderId = currentSlider.Id,
                Price = currentSlider.Price,
                SliderImage = currentSlider.SliderImage,
                SliderText = currentSlider.SliderText,
                SliderTitle = currentSlider.SliderTitle,
                TextBtn = currentSlider.TextBtn
            };
        }

        public async Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider)
        {
            var slider = await _siteSettingRepository.GetSliderById(editSlider.SliderId);

            if (slider==null) return EditSliderResult.NotFound;
            slider.SliderText = editSlider.SliderText;
            slider.SliderTitle = editSlider.SliderTitle;
            slider.TextBtn = editSlider.TextBtn;
            slider.Price = editSlider.Price;
            slider.Href = editSlider.Href;

            if (editSlider.ImageFile !=null&&editSlider.ImageFile.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(editSlider.ImageFile.FileName);
                editSlider.ImageFile.AddImageToServer(imageName, PathExtensions.SliderOrginServer, 255, 273,
                    PathExtensions.SliderThumbServer , slider.SliderImage);
                slider.SliderImage = imageName;
            }

            _siteSettingRepository.UpdateSlider(slider);
            await _siteSettingRepository.SaveChanges();
            return EditSliderResult.Success;
        }

        #endregion


    }
}