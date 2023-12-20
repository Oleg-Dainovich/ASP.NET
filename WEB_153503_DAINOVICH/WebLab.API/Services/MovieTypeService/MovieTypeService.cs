using Microsoft.EntityFrameworkCore;
using WebLab.API.Data;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;
using System.Linq;

namespace WebLab.API.Services.MovieTypeService
{
	public class MovieTypeService : IMovieTypeService
	{
		private readonly AppDbContext _context;

		public MovieTypeService(AppDbContext context)
		{
			_context = context;
		}
        public Task<ResponseData<List<MovieType>>> GetMovieTypeListAsync()
        {
            var movieTypes = _context.MovieType.ToList();
            var result = new ResponseData<List<MovieType>>(movieTypes);
            return Task.FromResult(result);
        }

    }
}
