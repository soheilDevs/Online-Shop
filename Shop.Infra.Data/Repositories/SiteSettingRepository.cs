﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Site.Sliders;
using Shop.Infra.Data.Context;

namespace Shop.Infra.Data.Repositories
{
    public class SiteSettingRepository:ISiteSettingRepository
    {
        #region Constractor

        private readonly ShopDbContext _context;

        public SiteSettingRepository(ShopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Slider

        public async Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filter)
        {
            var query = _context.Sliders.AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.SliderTitle))
            {
                query = query.Where(c => EF.Functions.Like(c.SliderTitle, $"%{filter.SliderTitle}%"));
            }

            #endregion

            #region Paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity,
                filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetSliders(allData);
        }

        public async Task AddSlider(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
        }

        public async Task<Slider> GetSliderById(long sliderId)
        {
            return await _context.Sliders.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == sliderId);
        }

        public void UpdateSlider(Slider slider)
        {
            _context.Sliders.Update(slider);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}