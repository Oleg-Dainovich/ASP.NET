using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WEB_153503_DAINOVICH.Services.MovieService;
using WEB_153503_DAINOVICH.Services.MovieTypeService;

namespace WEB_153503_DAINOVICH.Areas.Admin.Pages
{
	public class DeleteModel : PageModel
	{
		private readonly IMovieService _movieService;

		public DeleteModel(IMovieService MovieService)
		{
			_movieService = MovieService;
		}

		[BindProperty]
		public WebLab.Domain.Entities.Movie Movie { get; set; } = default!;

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

		public async Task<IActionResult> OnPostAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var movie = await _movieService.GetMovieByIdAsync((int)id);

			if (movie.Data != null)
			{
				Movie = movie.Data;
				await _movieService.DeleteMovieAsync(movie.Data.Id);
			}

			return RedirectToPage("./Index");
		}
	}
}