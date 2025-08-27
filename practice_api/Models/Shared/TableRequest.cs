namespace practice_api.Models.Shared
{
    public class TableRequest
    {
        public int first { get; set; } = 0;
        public int rows { get; set; } = 10;

        public string? sortField { get; set; }
        public int? sortOrder { get; set; } // 1 = Asc, -1 = Desc
        public string? search { get; set; }
    }


}
