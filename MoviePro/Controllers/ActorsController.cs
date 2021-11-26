using Microsoft.AspNetCore.Mvc;
using MoviePro.Services.Interfaces;
using System.Threading.Tasks;

namespace MoviePro.Controllers
{
    public class ActorsController : Controller
    {
        private readonly IRemoteMovieService _movieService;
        private readonly IDataMappingService _dataMapping;

        public ActorsController(IRemoteMovieService movieService,
            IDataMappingService dataMapping)
        {
            _movieService = movieService;
            _dataMapping = dataMapping;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var actor = await _movieService.ActorDetailAsync(id);
            actor = _dataMapping.MapActorDetail(actor);

            return View(actor);
        }
    }
}
