namespace ITunesSearchApp.Models
{
    public class AdminFavoritesViewModel
    {
        public string? SearchTerm { get; set; }
        public List<Album> SearchResults { get; set; } = new();
        public List<FavoriteAlbum> CurrentFavorites { get; set; } = new();
    }
}