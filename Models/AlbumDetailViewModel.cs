namespace ITunesSearchApp.Models
{
    public class AlbumDetailViewModel
    {
        public Album? Album { get; set; }
        public List<Song> Songs { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}