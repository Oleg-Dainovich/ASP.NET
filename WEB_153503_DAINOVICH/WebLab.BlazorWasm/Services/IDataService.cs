using WebLab.Domain.Entities;

namespace WebLab.BlazorWasm.Services
{
    public interface IDataService
    {
        event Action DataChanged;
        List<MovieType> MovieTypes { get; set; }
        List<Movie> MovieList { get; set; }
        bool Success { get; set; }
        string ErrorMessage { get; set; }
        int TotalPages { get; set; }
        int CurrentPage { get; set; }
    public Task GetProductListAsync(string? categoryNormalizedName, int pageNo = 1);
        public Task GetProductByIdAsync(int id);
        public Task GetCategoryListAsync();
    }
}
