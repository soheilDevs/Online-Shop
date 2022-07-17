using System.Threading.Tasks;
using Shop.Domain.ViewModels.Site.Sliders;

namespace Shop.Application.Interfaces
{
    public interface ISiteSettingService
    {
        #region Slider

        Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filterSlidersViewModel);
        Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider);
        Task<EditSliderViewModel> GetEditSlider(long sliderId);
        Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider);

        #endregion
    }
}