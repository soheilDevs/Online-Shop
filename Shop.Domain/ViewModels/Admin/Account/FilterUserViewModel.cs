using System.Collections.Generic;
using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Admin.Account
{
    public class FilterUserViewModel:BasePaging 
    {
        public string PhoneNumber { get; set; }
        public List<User> Users { get; set; }


        #region Methods

        public FilterUserViewModel SetUsers(List<User> users)
        {
            this.Users = users;
            return this;
        }

        public FilterUserViewModel SetPaging(BasePaging paging)
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