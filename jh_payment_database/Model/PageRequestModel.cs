namespace jh_payment_database.Model
{
    public class PageRequestModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string QueryString { get; set; }
        public string SortBy { get; set; }
    }
}
