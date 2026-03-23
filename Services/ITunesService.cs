using System.Text.Json;
using ITunesSearchApp.Models;

namespace ITunesSearchApp.Services
{
    public class ITunesService
    {
        private readonly HttpClient _httpClient;

        public ITunesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Album>> SearchAlbumsAsync(string searchTerm)
        {
            var response = await _httpClient.GetAsync(
                $"https://itunes.apple.com/search?term={Uri.EscapeDataString(searchTerm)}&entity=album&limit=25");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ITunesSearchResponse<Album>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result?.Results ?? new List<Album>();
        }

        public async Task<List<Album>> GetTopAlbumsAsync()
{
    var url = "https://rss.marketingtools.apple.com/api/v2/us/music/most-played/50/albums.json";

    var response = await _httpClient.GetAsync(url);

    if (!response.IsSuccessStatusCode)
    {
        var errorBody = await response.Content.ReadAsStringAsync();
        throw new Exception($"Apple RSS request failed. Status: {(int)response.StatusCode} {response.ReasonPhrase}. Response: {errorBody}");
    }

    var content = await response.Content.ReadAsStringAsync();

    var result = JsonSerializer.Deserialize<TopAlbumsResponse>(
        content,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    if (result?.Feed?.Results == null)
    {
        return new List<Album>();
    }

    return result.Feed.Results.Select(album => new Album
    {
        CollectionId = long.TryParse(album.Id, out var parsedId) ? parsedId : -1,
        CollectionName = album.Name,
        ArtistName = album.ArtistName,
        ArtworkUrl100 = album.ArtworkUrl100,
        ReleaseDate = DateTime.TryParse(album.ReleaseDate, out var parsedDate)
            ? parsedDate
            : DateTime.MinValue,
        PrimaryGenreName = string.Empty
    }).ToList();
}
        public async Task<AlbumDetailViewModel?> GetAlbumDetailsAsync(long collectionId)
        {
            var response = await _httpClient.GetAsync(
                $"https://itunes.apple.com/lookup?id={collectionId}&entity=song");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ITunesSearchResponse<Song>>(
                content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result?.Results == null || result.Results.Count == 0)
            {
                return null;
            }

            var albumInfo = result.Results.FirstOrDefault(x => x.WrapperType == "collection");
            var songs = result.Results.Where(x => x.WrapperType == "track").ToList();

            if (albumInfo == null)
            {
                return null;
            }

            var album = new Album
            {
                CollectionId = albumInfo.CollectionId,
                CollectionName = albumInfo.CollectionName,
                ArtistName = albumInfo.ArtistName,
                ArtworkUrl100 = albumInfo.ArtworkUrl100,
                ReleaseDate = albumInfo.ReleaseDate,
                PrimaryGenreName = albumInfo.PrimaryGenreName
            };

            return new AlbumDetailViewModel
            {
                Album = album,
                Songs = songs
            };
        }

        public async Task<List<Song>> GetAlbumSongsAsync(long collectionId)
        {
            var details = await GetAlbumDetailsAsync(collectionId);
            return details?.Songs ?? new List<Song>();
        }
    }
}