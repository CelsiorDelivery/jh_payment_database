namespace jh_payment_database.Model
{
    public class PageRequestModel
    {
        public int PageSize { get; set; } = 20;
        public int PageNumber { get; set; } = 1;
        public string QueryString { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
    }
}
