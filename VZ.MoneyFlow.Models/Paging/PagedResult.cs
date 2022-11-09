using System.Collections.Generic;


namespace VZ.MoneyFlow.Models.Paging
{
    public class PagedResult<T>
    {
        public int TotalCountOfRecords { get; set; }
        public int PageNumber { get; set; }
        public int RecordsPerPage { get; set; }
        public List<T> Records { get; set; }
    }
}
