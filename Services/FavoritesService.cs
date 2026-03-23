using ITunesSearchApp.Models;
using Microsoft.Extensions.Configuration;
using Supabase;

namespace ITunesSearchApp.Services
{
    public class FavoritesService
    {
        private readonly Client _supabase;

        public FavoritesService(IConfiguration configuration)
        {
            var url = configuration["Supabase:Url"];
            var key = configuration["Supabase:Key"];

            _supabase = new Client(url!, key!);
            _supabase.InitializeAsync().Wait();
        }

        public async Task<List<FavoriteAlbum>> GetFavoritesAsync()
        {
            var response = await _supabase
                .From<FavoriteAlbum>()
                .Order("display_order", Supabase.Postgrest.Constants.Ordering.Ascending)
                .Get();

            return response.Models;
        }

        public async Task<FavoriteAlbum?> GetFavoriteByItunesIdAsync(long itunesCollectionId)
        {
            var response = await _supabase
                .From<FavoriteAlbum>()
                .Where(x => x.ItunesCollectionId == itunesCollectionId)
                .Get();

            return response.Models.FirstOrDefault();
        }

        public async Task AddFavoriteAsync(FavoriteAlbum album)
        {
            await _supabase.From<FavoriteAlbum>().Insert(album);
        }

        public async Task DeleteFavoriteAsync(long id)
        {
            var existing = await _supabase
                .From<FavoriteAlbum>()
                .Where(x => x.Id == id)
                .Get();

            var item = existing.Models.FirstOrDefault();
            if (item != null)
            {
                await item.Delete<FavoriteAlbum>();
            }
        }

        public async Task UpdateFavoriteAsync(FavoriteAlbum album)
        {
            await album.Update<FavoriteAlbum>();
        }

        public async Task<FavoriteAlbum?> GetFavoriteByIdAsync(long id)
        {
            var response = await _supabase
                .From<FavoriteAlbum>()
                .Where(x => x.Id == id)
                .Get();

            return response.Models.FirstOrDefault();
        }
    }
}