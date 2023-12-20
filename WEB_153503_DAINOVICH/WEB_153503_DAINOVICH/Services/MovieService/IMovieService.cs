using WebLab.Domain.Models;
using WebLab.Domain.Entities;

namespace WEB_153503_DAINOVICH.Services.MovieService
{
    public interface IMovieService
    {
        public Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? movieTypeNormalizedName = null, int pageNo = 1, int pageSize = -1);
        public Task<ResponseData<Movie>> GetMovieByIdAsync(int id);
        public Task UpdateMovieAsync(int id, Movie movie, IFormFile? formFile);
        public Task DeleteMovieAsync(int id);
        public Task<ResponseData<Movie>> CreateMovieAsync(Movie movie, IFormFile? formFile);
    }
}
