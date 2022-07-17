﻿using System.Collections.Generic;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Account
{
    public class UserComparesViewModel:BasePaging
    {
        public List<UserCompare> UserCompares { get; set; }

        #region Methods

        public UserComparesViewModel SetCompares(List<UserCompare> userCompares)
        {
            this.UserCompares = userCompares;
            return this;
        }

        public UserComparesViewModel SetPaging(BasePaging paging)
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