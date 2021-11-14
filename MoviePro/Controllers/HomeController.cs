using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoviePro.Data;
using MoviePro.Enums;
using MoviePro.Models;
using MoviePro.Models.Settings;
using MoviePro.Models.ViewModels;
using MoviePro.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoviePro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRemoteMovieService _movieService;

        public HomeController(ILogger<HomeController> logger,
            ApplicationDbContext context,
            IRemoteMovieService movieService)
        {
            _logger = logger;
            _context = context;
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            const int count = 16;

            var data = new LandingPageVM()
            {
                CustomCollections = await _context.Collection.Include(c => c.MovieCollections)
                    .ThenInclude(mc => mc.Movie)
                    .ToListAsync(),
                NowPlaying = await _movieService.SearchMoviesAsync(MovieCategory.now_playing, count),
                Popular = await _movieService.SearchMoviesAsync(MovieCategory.popular, count),
                TopRated = await _movieService.SearchMoviesAsync(MovieCategory.top_rated, count),
                Upcoming = await _movieService.SearchMoviesAsync(MovieCategory.upcoming, count)
            };

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
