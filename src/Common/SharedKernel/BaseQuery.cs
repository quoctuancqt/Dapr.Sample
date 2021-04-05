using SharedKernel.Enums;
using System;

namespace SharedKernel
{
    public class BaseQuery
    {
        private int _pageIndex = 0;

        private int _pageSize = 20;

        private DateTime? _endDate;

        public int? PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                if (value.HasValue)
                {
                    _pageIndex = value.Value;
                }
            }
        }

        public int? PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value.HasValue)
                {
                    _pageSize = value.Value;
                }
            }
        }

        public string SearchText { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (value.HasValue)
                {
                    _endDate = value.Value.AddDays(1).AddTicks(-1);
                }
            }
        }

        public string[] OrderBy { get; set; } = Array.Empty<string>();

        public OrderingDirection Direction { get; set; } = OrderingDirection.Desc;
    }
}
