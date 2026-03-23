using ITunesSearchApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITunesSearchApp.Controllers
{
    public class AlbumController : Controller
    {
        private readonly ITunesService _iTunesService;

        public AlbumController(ITunesService iTunesService)
        {
            _iTunesService = iTunesService;
        }

        public async Task<IActionResult> Index(string term = "Taylor Swift")
        {
            ViewData["CurrentSearchTerm"] = term;

            var searchResults = await _iTunesService.SearchAlbumsAsync(term);
            return View(searchResults);
        }

        public async Task<IActionResult> Details(long id, string source = "search")
        {
            var viewModel = await _iTunesService.GetAlbumDetailsAsync(id);

            if (viewModel == null)
            {
                return NotFound();
            }

            viewModel.Songs ??= new List<ITunesSearchApp.Models.Song>();
            ViewData["Source"] = source;

            return View(viewModel);
        }
    }
}