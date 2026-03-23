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