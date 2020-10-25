using System;



namespace TableQuery.Entities{


    public  class GridResponse: PagedResult
    {
        public TableView ViewInfo { get; set; }


        public static GridResponse Convert(PagedResult pagedResponse)
        {
            var grid = new GridResponse()
            {
                Data = pagedResponse.Data,
                PageNumber = pagedResponse.PageNumber,
                PageSize = pagedResponse.PageSize,
                TotalPages = pagedResponse.TotalPages,
                NextPage = pagedResponse.NextPage,
                PreviousPage = pagedResponse.PreviousPage,
                TotalRecords = pagedResponse.TotalRecords,
                Filters = pagedResponse.Filters
            };

            return grid;
        }
    }
}