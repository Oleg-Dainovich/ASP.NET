using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebLab.Domain.Entities;
using WEB_153503_DAINOVICH.Services.MovieService;
using WEB_153503_DAINOVICH.Services.MovieTypeService;

namespace WEB_153503_DAINOVICH.Areas.Admin.Pages
{
	public class EditModel : PageModel
	{
		private readonly IMovieService _movieService;
		private readonly IMovieTypeService _movieTypeService;

		public EditModel(IMovieService MovieService, IMovieTypeService movieTypeService)
		{
			_movieService = MovieService;
			_movieTypeService = movieTypeService;
		}

		[BindProperty]
		public Movie Movie { get; set; } = default!;

		[BindProperty]
		public IFormFile? Image { get; set; }

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

			Movie = movie.Data;
			var movieTypes = await _movieTypeService.GetMovieTypeListAsync();
			ViewData["TypeId"] = new SelectList(movieTypes.Data, "Id", "Name");
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			await _movieService.UpdateMovieAsync(Movie.Id, Movie, Image);

			return RedirectToPage("./Index");
		}
	}
}