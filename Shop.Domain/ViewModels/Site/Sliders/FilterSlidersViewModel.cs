using System.Collections.Generic;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Site.Sliders
{
    public class FilterSlidersViewModel:BasePaging   
    {
        public string SliderTitle { get; set; }
        public List<Slider> Sliders { get; set; }

        #region Methods

        public FilterSlidersViewModel SetSliders(List<Models.Site.Slider> sliders)
        {
            this.Sliders = sliders;
            return this;
        }

        public FilterSlidersViewModel SetPaging(BasePaging paging)
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
}