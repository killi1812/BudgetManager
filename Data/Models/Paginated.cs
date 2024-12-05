namespace Data.Models;

public class Paginated<T>
{
    public IList<T> Items { get; set; }
    public int Page { get; set; }
    public int Count { get; set; }
    public int Size { get; set; }
    public int LastPage { get; set; }
    public int FromPage { get; set; }
    public int ToPage { get; set; }


    public Paginated(IEnumerable<T> items, int page = 1, int count = 10)
    {
        Count = items.Count();
        Items = items.Skip((page - 1) * count).Take(count).ToList();
        Page = page;
        Size = count;
        LastPage = (int)Math.Ceiling(Count / (double)count);
        FromPage = page - 5 > 0 ? page - 5 : 1;
        ToPage = Count / count >= page + 5 ? page + 5 : LastPage;
    }
}