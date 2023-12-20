using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLab.Domain.Entities;
using WEB_153503_DAINOVICH.Services.MovieService;

namespace WEB_153503_DAINOVICH.Areas.Admin.Pages
{
	public class DetailsModel : PageModel
	{
		private readonly IMovieService _movieService;

		public DetailsModel(IMovieService MovieService)
		{
			_movieService = MovieService;
		}

		[BindProperty]
		public Movie Movie { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var movie = await _movieService.GetMovieByIdAsync((int)id);
			if (movie.Data == null)
			{
				return NotFound();
			}
			else
			{
				Movie = movie.Data;
			}
			return Page();
		}
	}
}