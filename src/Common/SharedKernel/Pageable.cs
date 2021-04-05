using System;
using System.Collections.Generic;

namespace SharedKernel
{
    public class Pageable<T> where T : class
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public int TotalPage { get; set; }

        public Pageable(long totalItems, int take, int index, IEnumerable<T> items)
        {
            TotalItems = totalItems;
            TotalPage = (int)Math.Ceiling((double)(TotalItems / (double)take));
            PageSize = take;
            PageIndex = index;
            Items.AddRange(items);
        }
    }
}
