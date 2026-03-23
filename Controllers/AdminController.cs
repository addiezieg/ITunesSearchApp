using ITunesSearchApp.Models;
using ITunesSearchApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ITunesSearchApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ITunesService _itunesService;
        private readonly FavoritesService _favoritesService;

        public AdminController(ITunesService itunesService, FavoritesService favoritesService)
        {
            _itunesService = itunesService;
            _favoritesService = favoritesService;
        }

        [HttpGet]
        public async Task<IActionResult> Favorites(string? searchTerm)
        {
            var vm = new AdminFavoritesViewModel
            {
                SearchTerm = searchTerm,
                CurrentFavorites = await _favoritesService.GetFavoritesAsync()
            };

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var result = await _itunesService.SearchAlbumsAsync(searchTerm);
                vm.SearchResults = result ?? new List<Album>();
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite(long itunesCollectionId, string collectionName,
            string artistName, string? artworkUrl100, DateTime? releaseDate,
            string? primaryGenreName, string? collectionViewUrl)
        {
            var existing = await _favoritesService.GetFavoriteByItunesIdAsync(itunesCollectionId);
            if (existing == null)
            {
                await _favoritesService.AddFavoriteAsync(new FavoriteAlbum
                {
                    ItunesCollectionId = itunesCollectionId,
                    CollectionName = collectionName,
                    ArtistName = artistName,
                    ArtworkUrl100 = artworkUrl100,
                    ReleaseDate = releaseDate,
                    PrimaryGenreName = primaryGenreName,
                    CollectionViewUrl = collectionViewUrl,
                    DisplayOrder = 0,
                    IsFeatured = false
                });
            }

            return RedirectToAction("Favorites");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFavorite(long id)
        {
            await _favoritesService.DeleteFavoriteAsync(id);
            return RedirectToAction("Favorites");
        }

        [HttpGet]
        public async Task<IActionResult> EditFavorite(long id)
        {
            var favorite = await _favoritesService.GetFavoriteByIdAsync(id);
            if (favorite == null) return NotFound();

            return View(favorite);
        }

        [HttpPost]
        public async Task<IActionResult> EditFavorite(FavoriteAlbum model)
        {
            var existing = await _favoritesService.GetFavoriteByIdAsync(model.Id);
            if (existing == null) return NotFound();

            existing.Note = model.Note;
            existing.DisplayOrder = model.DisplayOrder;
            existing.IsFeatured = model.IsFeatured;

            await _favoritesService.UpdateFavoriteAsync(existing);

            return RedirectToAction("Favorites");
        }
    }
}