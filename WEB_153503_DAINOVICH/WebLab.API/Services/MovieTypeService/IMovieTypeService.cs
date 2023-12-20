using WebLab.Domain.Models;
using WebLab.Domain.Entities;

namespace WebLab.API.Services.MovieTypeService
{
	public interface IMovieTypeService
	{
		public Task<ResponseData<List<MovieType>>> GetMovieTypeListAsync();
	}
}
