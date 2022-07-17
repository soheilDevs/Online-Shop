using System.Linq;
using Shop.Domain.ViewModels.Paging;

namespace Shop.Domain.ViewModels.Account
{
    public static class PagingExtentions
    {
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, BasePaging basePaging)
        {
            return query.Skip(basePaging.SkipEntity).Take(basePaging.TakeEntity);
        }
    }
}