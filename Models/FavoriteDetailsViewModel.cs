namespace ITunesSearchApp.Models
{
    public class FavoriteDetailsViewModel
    {
        public FavoriteAlbum Favorite { get; set; } = null!;
        public List<Song> Songs { get; set; } = new();
    }
}