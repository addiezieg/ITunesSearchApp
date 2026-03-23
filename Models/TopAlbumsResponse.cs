using System.Text.Json.Serialization;

namespace ITunesSearchApp.Models
{
    public class TopAlbumsResponse
    {
        [JsonPropertyName("feed")]
        public TopAlbumsFeed Feed { get; set; } = new();
    }

    public class TopAlbumsFeed
    {
        [JsonPropertyName("results")]
        public List<TopAlbumItem> Results { get; set; } = new();
    }

    public class TopAlbumItem
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("artistName")]
        public string ArtistName { get; set; } = string.Empty;

        [JsonPropertyName("artworkUrl100")]
        public string ArtworkUrl100 { get; set; } = string.Empty;

        [JsonPropertyName("releaseDate")]
        public string ReleaseDate { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}