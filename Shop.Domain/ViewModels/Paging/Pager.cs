using System;

namespace Shop.Domain.ViewModels.Paging
{
    public static class Pager
    {
        public static BasePaging Build(int pageId, int allEntityCount, int take, int countForShowAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntityCount / (double) take));

            return new BasePaging
            {
                PageId = pageId,
                AllEntityCount = allEntityCount,
                CountForShowAfterAndBefore = countForShowAfterAndBefore,
                SkipEntity = (pageId - 1) * take,
                TakeEntity = take,
                StartPage = pageId - countForShowAfterAndBefore <= 0 ? 1 : pageId - countForShowAfterAndBefore,
                EndPage = pageId + countForShowAfterAndBefore > pageCount
                    ? pageCount
                    : pageId + countForShowAfterAndBefore,
                PageCount = pageCount
            };
        }
    }
}