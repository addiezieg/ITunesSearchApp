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
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = "Taylor Swift";
            }

            var url = $"https://itunes.apple.com/search?term={Uri.EscapeDataString(searchTerm)}&entity=album&limit=25";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ITunesSearchResponse<Album>>(
                    content,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return result?.Results ?? new List<Album>();
            }
            catch
            {
                return new List<Album>();
            }
        }

        public async Task<AlbumDetailViewModel> GetAlbumDetailsAsync(int collectionId)
        {
            var model = new AlbumDetailViewModel();

            var url = $"https://itunes.apple.com/lookup?id={collectionId}&entity=song";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(content);
                var results = doc.RootElement.GetProperty("results");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                foreach (var item in results.EnumerateArray())
                {
                    if (item.TryGetProperty("wrapperType", out var wrapperType))
                    {
                        var wrapperTypeValue = wrapperType.GetString();

                        if (wrapperTypeValue == "collection")
                        {
                            var album = JsonSerializer.Deserialize<Album>(item.GetRawText(), options);
                            if (album != null)
                            {
                                model.Album = album;
                            }
                        }
                        else if (wrapperTypeValue == "track")
                        {
                            var song = JsonSerializer.Deserialize<Song>(item.GetRawText(), options);
                            if (song != null)
                            {
                                model.Songs.Add(song);
                            }
                        }
                    }
                }

                if (model.Album == null)
                {
                    model.ErrorMessage = "Album not found.";
                }
            }
            catch
            {
                model.ErrorMessage = "Error loading album details.";
            }

            return model;
        }
    }
}