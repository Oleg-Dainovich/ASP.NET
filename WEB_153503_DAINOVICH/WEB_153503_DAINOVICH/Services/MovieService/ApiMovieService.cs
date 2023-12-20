using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WEB_153503_DAINOVICH.Services.MovieService;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;

namespace WebLab.Services.MovieService
{
	public class ApiMovieService : IMovieService
	{
		HttpClient _httpClient;
		private readonly HttpContext _httpContext;

		JsonSerializerOptions _serializerOptions;
		ILogger _logger;


		int _pageSize;

		public ApiMovieService(IHttpClientFactory httpClientFactory, [FromServices] IConfiguration config, ILogger<ApiMovieService> logger, IHttpContextAccessor httpContextAccessor)
		{
			_pageSize = int.Parse(config["MoviePageSize"]!);
			_httpClient = httpClientFactory.CreateClient("API");
			_logger = logger;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				IncludeFields = true,
			};
			_httpContext = httpContextAccessor.HttpContext;

		}

		public async Task<ResponseData<Movie>> CreateMovieAsync(Movie movie, IFormFile? formFile)
		{
			var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}movies";
			var uri = new Uri(urlString);

			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var response = await _httpClient.PostAsJsonAsync(uri, movie, _serializerOptions);

			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = $"Object not created. Error: {response.StatusCode}";
				_logger.LogError(errorMessage);

				return new ResponseData<Movie>(null!)
				{
					 Success = false,
					ErrorMessage = errorMessage,
				};

			}

			var data = await response.Content.ReadFromJsonAsync<ResponseData<Movie>>(_serializerOptions);

			if (formFile != null && data != null)
			{
				await SaveImageAsync(data.Data.Id, formFile);
			}

			return data!;
		}

		public async Task DeleteMovieAsync(int id)
		{
			var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}movies/{id}";
			var uri = new Uri(urlString);

			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var response = await _httpClient.DeleteAsync(uri);
			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = $"Object not deleted. Error: {response.StatusCode}";
				_logger.LogError(errorMessage);
			}
		}

		public async Task<ResponseData<ListModel<Movie>>> GetMovieListAsync(string? movieTypeNormalized, int pageNo = 1, int pageSize = -1)
		{
			pageSize = pageSize == -1 ? _pageSize : pageSize;
			var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}movies");
			if (pageNo > 1)
			{
				urlString.Append($"/page{pageNo}");
			};

			var query = new List<KeyValuePair<string, string?>>();
			if (pageSize != 3)
			{
				query.Add(new("pageSize", pageSize.ToString()));
			}
			if (movieTypeNormalized != null)
			{
				query.Add(new("movieType", movieTypeNormalized));
			};
			
			if (query.Count > 0)
			{
				urlString.Append(QueryString.Create(query));
			}

			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = $"Object movie not recieved. Error {response.StatusCode}";
				_logger.LogError(errorMessage);

				return new ResponseData<ListModel<Movie>>(null!)
				{
					Success = false,
					ErrorMessage = errorMessage,
				};
			}

			var movieList = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Movie>>>(_serializerOptions);
			return movieList!;

		}


		public async Task<ResponseData<Movie>> GetMovieByIdAsync(int id)
		{
			var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}movies/{id}";
			var uri = new Uri(urlString);


			var response = await _httpClient.GetAsync(uri);

			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = $"Object not recieved. Error {response.StatusCode}";
				_logger.LogError(errorMessage);

				return new ResponseData<Movie>(null!)
				{
					Success = false,
					ErrorMessage = errorMessage,
				};

			}

			var data = await response.Content.ReadFromJsonAsync<ResponseData<Movie>>(_serializerOptions);
			return data!;
		}

		public async Task UpdateMovieAsync(int id, Movie movie, IFormFile? formFile)
		{
			var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}movies/{id}";
			var uri = new Uri(urlString);

			var token = await _httpContext.GetTokenAsync("access_token");
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			var response = await _httpClient.PutAsJsonAsync(uri, movie, _serializerOptions);

			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = $"Object not updated. Error {response.StatusCode}";
				_logger.LogError(errorMessage);
			}

			if (formFile != null)
			{
				await SaveImageAsync(id, formFile);
			}
		}

		private async Task SaveImageAsync(int id, IFormFile image)
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}movies/{id}"),
			};
			var content = new MultipartFormDataContent();
			var streamContent = new StreamContent(image.OpenReadStream());
			content.Add(streamContent, "formFile", image.FileName);
			request.Content = content;
			var token = await _httpContext.GetTokenAsync("access_token");
			request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
			await _httpClient.SendAsync(request);
		}
	}
}