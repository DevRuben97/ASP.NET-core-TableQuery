using System;
using System.Collections.Generic;
using System.Text;

namespace TableQuery.Entities
{
    public class PagedResult
    {
        public object Data { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages { get; set; }

        public int NextPage { get; set; }

        public int? PreviousPage { get; set; }

        public int TotalRecords { get; set; }

        public List<FilterQuery> Filters { get; set; }
    }
}
