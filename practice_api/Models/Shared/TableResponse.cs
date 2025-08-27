namespace practice_api.Models.Shared
{
    public class TableResponse<T>
    {
        public IEnumerable<T> data { get; set; }
        public int totalRecords { get; set; }
    }
}
