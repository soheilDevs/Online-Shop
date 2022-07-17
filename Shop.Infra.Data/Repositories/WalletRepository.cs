using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Account;
using Shop.Domain.ViewModels.Paging;
using Shop.Domain.ViewModels.Wallet;
using Shop.Infra.Data.Context;

namespace Shop.Infra.Data.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ShopDbContext _context;

        #region Constractor

        public WalletRepository(ShopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Wallet

        public async Task CreateWallet(UserWallet userWallet)
        {
            await _context.UserWallets.AddAsync(userWallet);
        }

        public async Task<UserWallet> GetUserWalletById(long walletId)
        {
            return await _context.UserWallets.AsQueryable()
                .SingleOrDefaultAsync(c => c.Id == walletId);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateWallet(UserWallet wallet)
        {
            _context.UserWallets.Update(wallet);
        }

        public async Task<FilterWalletViewModel> FilterWallets(FilterWalletViewModel filter)
        {
            var query = _context.UserWallets.AsQueryable();

            #region  Filter

            if (filter.UserId != 0 && filter.UserId != null)
            {
                query = query.Where(c => c.UserId == filter.UserId);
            }
            #endregion

            #region Paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity,
                filter.CountForShowAfterAndBefore);
            var allData = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetWallets(allData);
        }

        public async Task<int> GetUserWalletAmount(long userId)
        {
            var variz = await _context.UserWallets
                .Where(c => c.UserId == userId && c.WalletType == WalletType.Variz && c.IsPay)
                .Select(c => c.Amount).ToListAsync();

            var bardasht = await _context.UserWallets
                .Where(c => c.UserId == userId && c.WalletType == WalletType.Bardasht && c.IsPay)
                .Select(c => c.Amount).ToListAsync();

            return (variz.Sum() - bardasht.Sum());
        }

        #endregion
    }
}