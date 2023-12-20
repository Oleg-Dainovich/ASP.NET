using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WebLab.API.Data;
using WebLab.API.Services.MovieService;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;

namespace WebLab.Tests
{
	public class ApiMovieServiceTests : IDisposable
	{
		private readonly DbConnection _connection;
		private readonly DbContextOptions<AppDbContext> _contextOptions;

		public ApiMovieServiceTests()
		{
			_connection = new SqliteConnection("Data Source=:memory:");
			_connection.Open();

			_contextOptions = new DbContextOptionsBuilder<AppDbContext>()
				.UseSqlite(_connection)
				.Options;

			using var context = new AppDbContext(_contextOptions);

			context.Database.EnsureCreated();

			MovieType movieType1 = new() { Name = "Movie1", NormalizedName = "movie1" };
			MovieType movieType2 = new() { Name = "Movie2", NormalizedName = "movie2" };
			context.MovieType.AddRange(movieType1, movieType2);
			context.SaveChanges();

			List<Movie> movies = new();
			for (int i = 0; i < 30; i++)
			{
				Movie movie = new() { Title = $"Beew {i}", Description = $"Description {i}", Price = i, TypeId = i % 2 + 1 };
				movies.Add(movie);
			}
			context.AddRange(movies);

			context.SaveChanges();
		}

		AppDbContext CreateContext() => new AppDbContext(_contextOptions);

		public void Dispose() => _connection.Dispose();

		[Fact]
		public void GetMovieListAsync_NoFilter_ReturnsFirstPageOfThreeItems()
		{
			using var context = CreateContext();
			var service = new MovieService(context, null!, null!);
			var result = service.GetMovieListAsync(null).Result;
			Assert.True(result.Success);
			Assert.IsType<ResponseData<ListModel<Movie>>>(result);
			Assert.Equal(1, result.Data?.CurrentPage);
			Assert.Equal(3, result.Data?.Items.Count);
			Assert.Equal(10, result.Data?.TotalPages);
			Assert.Equal(context.Movie.AsEnumerable().Take(3), result.Data?.Items);
		}

		[Fact]
		public void GetMovieListAsync_WithPageNumber_ReturnsRightPageOfThreeItems()
		{
			using var context = CreateContext();
			var service = new MovieService(context, null!, null!);
			var result = service.GetMovieListAsync(null, pageNo: 2).Result;
			Assert.True(result.Success);
			Assert.IsType<ResponseData<ListModel<Movie>>>(result);
			Assert.Equal(2, result.Data?.CurrentPage);
			Assert.Equal(3, result.Data?.Items.Count);
			Assert.Equal(10, result.Data?.TotalPages);
			Assert.Equal(context.Movie.AsEnumerable().Skip(3).Take(3), result.Data?.Items);
		}

		[Fact]
		public void GetMovieListAsync_WithCategoryFilter_ReturnsFilteredByCategory()
		{
			using var context = CreateContext();
			var service = new MovieService(context, null!, null!);
			var result = service.GetMovieListAsync("movie1").Result;
			Assert.True(result.Success);
			Assert.IsType<ResponseData<ListModel<Movie>>>(result);
			Assert.Equal(1, result.Data?.CurrentPage);
			Assert.Equal(3, result.Data?.Items.Count);
			Assert.Equal(5, result.Data?.TotalPages);
			Assert.Equal(context.Movie.AsEnumerable().Where((b) => b.TypeId == 1).Take(3), result.Data?.Items);
		}

		[Fact]
		public void GetMovieListAsync_MaxSizeSucceded_ReturnsMaximumMaxSize()
		{
			using var context = CreateContext();
			var service = new MovieService(context, null!, null!);
			var result = service.GetMovieListAsync(null, pageSize: 30).Result;
			Assert.True(result.Success);
			Assert.IsType<ResponseData<ListModel<Movie>>>(result);
			Assert.Equal(1, result.Data?.CurrentPage);
			Assert.Equal(20, result.Data?.Items.Count);
			Assert.Equal(2, result.Data?.TotalPages);
		}

		[Fact]
		public void GetMovieListAsync_PageNoIncorrect_ReturnsError()
		{
			using var context = CreateContext();
			var service = new MovieService(context, null!, null!);
			var result = service.GetMovieListAsync(null, pageNo: 15).Result;
			Assert.False(result.Success);
		}
	}
}