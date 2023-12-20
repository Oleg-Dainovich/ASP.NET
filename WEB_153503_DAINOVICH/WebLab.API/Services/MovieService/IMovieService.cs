using WebLab.Domain.Models;
using WebLab.Domain.Entities;

namespace WebLab.API.Services.MovieService
{
	public interface IMovieService
	{
		public Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? movieTypeNormalizedName, int pageNo = 1, int pageSize = 3);
		public Task<ResponseData<Movie>> GetMovieByIdAsync(int id);

		public Task UpdateMovieAsync(int id, Movie movie);
		public Task DeleteMovieAsync(int id);
		public Task<ResponseData<Movie>> CreateMovieAsync(Movie movie);
		public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
	}
}
