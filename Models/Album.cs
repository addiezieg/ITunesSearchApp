namespace ITunesSearchApp.Models
{
    public class Album
    {
        public int CollectionId { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string ArtworkUrl100 { get; set; } = string.Empty;
        public string PrimaryGenreName { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        public List<Song> Songs { get; set; } = new();
    }
}