using ITunesSearchApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITunesSearchApp.Controllers
{
    public class PopularController : Controller
    {
        private readonly ITunesService _iTunesService;

        public PopularController(ITunesService iTunesService)
        {
            _iTunesService = iTunesService;
        }

        public async Task<IActionResult> Index()
        {
            var topAlbums = await _iTunesService.GetTopAlbumsAsync();
            return View(topAlbums);
        }
    }
}