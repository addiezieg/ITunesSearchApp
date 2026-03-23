namespace ITunesSearchApp.Models
{
    public class ITunesSearchResponse<T>
    {
        public int ResultCount { get; set; }
        public List<T> Results { get; set; } = new();
    }
}