using Microsoft.EntityFrameworkCore;
using WebLab.API.Data;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;

namespace WebLab.API.Services.MovieService
{
    public class MovieService : IMovieService
    {

        private readonly int _maxPageSize = 20;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IWebHostEnvironment _environment;
        private readonly string _imageFolder;

        public MovieService(AppDbContext context, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
            _environment = environment;
            _imageFolder = Path.Combine(environment?.WebRootPath ?? "", "Images");
        }

        public async Task<ResponseData<Movie>> CreateMovieAsync(Movie movie)
        {
            _context.Movie.Add(movie);
            await _context.SaveChangesAsync();
            return new ResponseData<Movie>(movie);
        }

        public async Task DeleteMovieAsync(int id)
        {
            var wineToRemove = _context.Movie.Find(id) ?? throw new KeyNotFoundException();
            _context.Remove(wineToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? movieTypeNormalized, int pageNo = 1, int pageSize = 3)
        {
            if (pageSize > _maxPageSize)
                pageSize = _maxPageSize;
            var query = _context.Movie.AsQueryable();
            var dataList = new ListModel<Movie>();
            if (movieTypeNormalized != null)
            {
                query = query.Where(movie => movie.Type!.NormalizedName.Equals(movieTypeNormalized));

            }

            var count = query.Count();
            if (count == 0)
            {
                return new ResponseData<ListModel<Movie>>(dataList);
            }

            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return new ResponseData<ListModel<Movie>>(null!)
                {
                    Success = false,
                    ErrorMessage = "No such page"
                };
            }

            //dataList.Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).Include(movie => movie.Type).ToListAsync();
            dataList.Items = await query.Skip((pageNo - 1) * pageSize).Take(pageSize).OrderBy(movie => movie.Price).ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;

            return new ResponseData<ListModel<Movie>>(dataList)
            {
                Data = dataList
            };
        }

        public Task<ResponseData<Movie>> GetMovieByIdAsync(int id)
        {
            var movie = _context.Movie.Include(movie => movie.Type).FirstOrDefault(movie => movie.Id == id);
            return Task.FromResult(new ResponseData<Movie>(movie!));
        }

        public async Task UpdateMovieAsync(int id, Movie movie)
        {
            var types = _context.MovieType.ToList();
            var wineToUpdate = _context.Movie.Find(id) ?? throw new KeyNotFoundException();

            wineToUpdate.Price = movie.Price;
            wineToUpdate.Description = movie.Description;
            wineToUpdate.Title = movie.Title;
            wineToUpdate.TypeId = movie.TypeId;

            _context.Update(wineToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var responseData = new ResponseData<string>(String.Empty);
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                responseData.Success = false;
                responseData.ErrorMessage = "No item found";
                return responseData;
            }

            var host = "https://" + _contextAccessor.HttpContext!.Request.Host;
            if (formFile != null)
            {
                if (!string.IsNullOrEmpty(movie.Image))
                {
                    var prevImage = Path.GetFileName(movie.Image);
                    File.Delete(Path.Combine(_imageFolder, prevImage));
                }

                var ext = Path.GetExtension(formFile.FileName);
                var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);

                using var fileStream = File.Create(Path.Combine(_imageFolder, fName));
                await formFile.CopyToAsync(fileStream);

                movie.Image = $"{host}/images/{fName}";
                await _context.SaveChangesAsync();
            }
            responseData.Data = movie.Image;
            return responseData;
        }
    }

}
