namespace WebApp.ViewModels
{
    public class SearchVM<T>
    {
        public string Query { get; set; }
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public int Count { get; set; }
        public int LastPage { get; set; }
        public int FromPage { get; set; }
        public int ToPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}