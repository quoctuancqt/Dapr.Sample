using SharedKernel.Enums;
using System;

namespace SharedKernel
{
    public class BaseQuery
    {
        public int PageIndex { get; set; } = 0;

        public int PageSize { get; set; } = 10;

        public string SearchText { get; set; }

        public string[] SearchFields { get; set; } = Array.Empty<string>();

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string[] OrderBy { get; set; } = Array.Empty<string>();

        public OrderingDirection Direction { get; set; } = OrderingDirection.Desc;

        public virtual int GetSkip()
        {
            return PageIndex * PageSize;
        }

        public virtual int GetTake()
        {
            return PageSize;
        }
    }
}
