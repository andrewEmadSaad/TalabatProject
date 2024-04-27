using System.Collections.Generic;
using Talabat.Apis.Dtos;

namespace Talabat.Apis.Helpers
{
    public class Pagination<T>
    {
        public Pagination(int pageIndex, int pageSize, IReadOnlyList<T> date,int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = date;
            Count = count;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int Count { get; set; }

        public IReadOnlyList<T> Data { get; set; }
    }
}
