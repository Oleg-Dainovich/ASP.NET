using WebLab.Domain.Models;
using WebLab.Domain.Entities;

namespace WEB_153503_DAINOVICH.Services.MovieTypeService
{
    public interface IMovieTypeService
    {
        public Task<ResponseData<List<MovieType>>> GetMovieTypeListAsync();
    }
}
