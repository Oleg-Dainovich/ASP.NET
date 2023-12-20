using Microsoft.AspNetCore.Mvc.RazorPages;
using WebLab.Domain.Entities;
using WEB_153503_DAINOVICH.Services.MovieService;
using WebLab.Domain.Models;

namespace WEB_153503_DAINOVICH.Areas.Admin.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IMovieService _movieService;
		public IndexModel(IMovieService MovieService)
		{
			_movieService = MovieService;
		}

		public ListModel<Movie> Movie { get; set; } = default!;

		public async Task OnGetAsync(int pageNo = 1)
		{
			var movies = await _movieService.GetMovieListAsync(pageNo: pageNo);
			Movie = movies.Data;
		}
	}
}