namespace ITunesSearchApp.Models
{
    public class Song
    {
        public string? WrapperType { get; set; }

        public long CollectionId { get; set; }
        public string CollectionName { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string? ArtworkUrl100 { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? PrimaryGenreName { get; set; }

        public int TrackNumber { get; set; }
        public string TrackName { get; set; } = string.Empty;
        public int? TrackTimeMillis { get; set; }
    }
}