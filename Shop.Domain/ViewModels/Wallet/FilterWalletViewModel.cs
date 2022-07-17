using System.Collections.Generic;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Wallet
{
    public class FilterWalletViewModel:BasePaging
    {
        #region Properties

        public long? UserId { get; set; }
        public List<UserWallet> UserWallets { get; set; }   

        #endregion

        #region Methods

        public FilterWalletViewModel SetWallets(List<UserWallet> userWallets)
        {
            this.UserWallets = userWallets;
            return this;
        }

        public FilterWalletViewModel SetPaging(BasePaging paging)
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