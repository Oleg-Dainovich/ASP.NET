using NuGet.Protocol;
using System.Text.Json;
using System.Text;
using WebLab.Domain.Models;
using WebLab.Domain.Entities;

namespace WEB_153503_DAINOVICH.Services.MovieTypeService
{
	public class ApiMovieTypeService : IMovieTypeService
	{
		HttpClient _httpClient;
		ILogger _logger;
		JsonSerializerOptions _serializerOptions;

		public ApiMovieTypeService(IHttpClientFactory httpClientFactory, ILogger<ApiMovieTypeService> logger)
		{
			_httpClient = httpClientFactory.CreateClient("API");
			_logger = logger;

			_serializerOptions = new JsonSerializerOptions()
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			};
		}

		public async Task<ResponseData<List<MovieType>>> GetMovieTypeListAsync()
		{
			var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}movietypes");

			var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));
			if (!response.IsSuccessStatusCode)
			{
				var errorMessage = $"Object not recieved. Error {response.StatusCode}";
				_logger.LogError(errorMessage);

				return new ResponseData<List<MovieType>>(null!)
				{
					Success = false,
					ErrorMessage = errorMessage,
				};
			}

			var movieTypeList = await response.Content.ReadFromJsonAsync<ResponseData<List<MovieType>>>(_serializerOptions);
			return movieTypeList!;
		}
	}
}
