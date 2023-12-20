using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB_153503_DAINOVICH.Services.MovieService;
using WEB_153503_DAINOVICH.Services.MovieTypeService;
using WebLab.Domain.Entities;


namespace WEB_153503_DAINOVICH.Areas.Admin.Pages
{
	public class CreateModel : PageModel
	{
		private readonly IMovieService _movieService;
		private readonly IMovieTypeService _movieTypeService;

		public CreateModel(IMovieService MovieService, IMovieTypeService movieTypeService)
		{
			_movieService = MovieService;
			_movieTypeService = movieTypeService;
		}

		public async Task<IActionResult> OnGet()
		{
			var movieTypes = await _movieTypeService.GetMovieTypeListAsync();
			ViewData["TypeId"] = new SelectList(movieTypes.Data, "Id", "Name");
			return Page();
		}

		[BindProperty]
		public Movie Movie { get; set; } = default!;

		[BindProperty]
		public IFormFile? Image { get; set; }


		// To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid || Movie == null)
			{
				return Page();
			}

			await _movieService.CreateMovieAsync(Movie, Image);

			return RedirectToPage("./Index");
		}
	}
}