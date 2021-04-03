using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WechatMall.Api.Helpers
{
    public class PagedList<T> : IEnumerable<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        private List<T> list;
        private PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            list = items;
        }

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public static PagedList<T> Empty = new PagedList<T>(new List<T>(1), 1, 1, 1);
    }
}
