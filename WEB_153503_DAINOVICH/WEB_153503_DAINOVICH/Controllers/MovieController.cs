using Microsoft.AspNetCore.Mvc;
using WEB_153503_DAINOVICH.Extensions;
using WEB_153503_DAINOVICH.Services.MovieService;
using WEB_153503_DAINOVICH.Services.MovieTypeService;

namespace WEB_153503_DAINOVICH.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMovieTypeService _movieTypeService;

        public MovieController(IMovieService MovieService, IMovieTypeService movieTypeService)
        {
            _movieService = MovieService;
            _movieTypeService = movieTypeService;
        }

		[Route("movie/{movieType?}")]
		public async Task<IActionResult> Index(string? movieType, int pageNo)
        {
            var movieTypes = await _movieTypeService.GetMovieTypeListAsync();

            if (!movieTypes.Success)
                return NotFound(movieTypes.ErrorMessage);

            ViewData["movieTypes"] = movieTypes.Data;

			var currentMovieType = movieType == null ? "Все" : movieTypes.Data.FirstOrDefault(c => c.NormalizedName == movieType)?.Name;

			ViewData["movieType"] = currentMovieType;

			var productResponse = await _movieService.GetMovieListAsync(movieType == "Все" ? null : movieType, pageNo);

			if (!productResponse.Success)
				return NotFound(productResponse.ErrorMessage);

			if (Request.IsAjaxRequest())
            {
                return PartialView("_MovieListPartial", productResponse.Data);
            }
            return View(productResponse.Data);
        }
    }
}
