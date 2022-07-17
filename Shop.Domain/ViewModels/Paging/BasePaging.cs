﻿namespace Shop.Domain.ViewModels.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            PageId = 1;
            TakeEntity = 15;
            CountForShowAfterAndBefore = 5;
        }

        public int PageId { get; set; }
        public int PageCount { get; set; }
        public int AllEntityCount { get; set; }
        public int CountForShowAfterAndBefore { get; set; }
        public int SkipEntity { get; set; }
        public int TakeEntity { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        public BasePaging GetCurrentPaging()
        {
            return this;
        }
    }
}