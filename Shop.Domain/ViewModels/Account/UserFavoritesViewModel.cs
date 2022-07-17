using System.Collections.Generic;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Account
{
    public class UserFavoritesViewModel:BasePaging
    {
        public List<UserFavorite> UserFavorites { get; set; }

        #region Methods

        public UserFavoritesViewModel SetFavorites(List<UserFavorite> userFavorites)
        {
            this.UserFavorites = userFavorites;
            return this;
        }

        public UserFavoritesViewModel SetPaging(BasePaging paging)
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