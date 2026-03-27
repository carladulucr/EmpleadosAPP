namespace EmpleadosAPI.DTOs
{
    public class PaginacionDTO<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int Pages { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}
