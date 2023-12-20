using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace WebLab.BlazorWasm.Services
{

    public class DataService : IDataService
    {
        public event Action DataChanged;
        private HttpClient _httpClient;
        private String _apiUri;
        private int _itemsPerPage;
        private JsonSerializerOptions _serializerOptions;

        public DataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiUri = configuration.GetValue<String>("ApiUri")!;
            _itemsPerPage = configuration.GetValue<int>("MoviesPerPage");

            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public List<MovieType> MovieTypes { get; set; } = new List<MovieType>();

        public List<Movie> MovieList { get; set; } = new List<Movie>();

        public bool Success { get; set; }

        public string ErrorMessage { get; set; } = "";

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public async Task GetCategoryListAsync()
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}movietypes");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = $"Object not recieved. Error {response.StatusCode}";
                Success = false;
                return;
            }

            MovieTypes = (await response.Content.ReadFromJsonAsync<ResponseData<List<MovieType>>>(_serializerOptions)).Data;
            Success = true;
        }

        public Task GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task GetProductListAsync(string? movieTypeNormalized, int pageNo = 1)
        {
            
            var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}movies");
            if (pageNo > 1)
            {
                urlString.Append($"/page{pageNo}");
            };

            var query = new List<KeyValuePair<string, string?>>();

            if (movieTypeNormalized != null)
            {
                query.Add(new("movieType", movieTypeNormalized));
            };
			if (query.Count > 0)
			{
				urlString.Append(QueryString.Create(query));
			}
			var a = urlString.ToString();
            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Object not recieved. Error {response.StatusCode}";

                MovieList = null;
                Success= false;
                ErrorMessage = errorMessage;

                return;
            }

            var responseData = (await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Movie>>>(_serializerOptions));
            MovieList = responseData?.Data?.Items;
            TotalPages = responseData?.Data?.TotalPages ?? 0;
            CurrentPage = responseData?.Data?.CurrentPage ?? 0;
            Success = true;
            DataChanged?.Invoke();
            return;
        }
    }
}
