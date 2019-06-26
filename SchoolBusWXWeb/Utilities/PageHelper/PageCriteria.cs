namespace SchoolBusWXWeb.Utilities.PageHelper
{
    public class PageCriteria
    {
        public string TableName { get; set; }
        public string Fields { get; set; } = "*";
        public string PrimaryKey { get; set; } = "ID";
        public int PageSize { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public string Sort { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;
        public int RecordCount { get; set; }
    }
}