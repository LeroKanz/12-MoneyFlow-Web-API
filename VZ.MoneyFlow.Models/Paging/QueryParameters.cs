using System.Collections.Generic;


namespace VZ.MoneyFlow.Models.Paging
{
    public class QueryParameters
    {
        private int _recordsPerPage = 15;
        public int PageNumber { get; set; }
        public int RecordsPerPage
        {
            get { return _recordsPerPage; }
            set { _recordsPerPage = value; }
        }
    }    
}
