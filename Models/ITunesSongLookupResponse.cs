namespace ITunesSearchApp.Models
{
    public class ITunesSongLookupResponse
    {
        public int ResultCount { get; set; }
        public List<Song> Results { get; set; } = new();
    }
}