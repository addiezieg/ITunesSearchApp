using ITunesSearchApp.Models;
using ITunesSearchApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITunesSearchApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly FavoritesService _favoritesService;
        private readonly ITunesService _itunesService;

        public FavoritesController(FavoritesService favoritesService, ITunesService itunesService)
        {
            _favoritesService = favoritesService;
            _itunesService = itunesService;
        }

        public async Task<IActionResult> Index()
        {
            var favorites = await _favoritesService.GetFavoritesAsync();
            return View(favorites);
        }

        public async Task<IActionResult> Details(long id)
        {
            var favorite = await _favoritesService.GetFavoriteByIdAsync(id);
            if (favorite == null) return NotFound();

            var songs = await _itunesService.GetAlbumSongsAsync(favorite.ItunesCollectionId);

            var vm = new FavoriteDetailsViewModel
            {
                Favorite = favorite,
                Songs = songs
            };

            return View(vm);
        }
    }
}