using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebLab.API.Data;
using WebLab.API.Services.MovieService;
using WebLab.API.Services.MovieTypeService;

namespace WebLab.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			AddServices(builder);

			AddDbContext(builder);

			WebApplication app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}



			app.UseHttpsRedirection();

			app.UseCors(opt =>
			{
				opt.AllowAnyOrigin();
			});


			app.UseStaticFiles();

			app.UseAuthentication();
			app.UseAuthorization();

           

			app.MapControllers();


			DbInitializer.SeedData(app).Wait();

			app.Run();
		}

		private static void AddDbContext(WebApplicationBuilder builder)
		{
			//var connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new InvalidOperationException("Connection string 'Default' not found.");
            var configBuilder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json");
            IConfiguration _configuration = configBuilder.Build();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }
                ));
        }

		private static void AddServices(WebApplicationBuilder builder)
		{
			builder.Services.AddScoped<IMovieTypeService, MovieTypeService>();
			builder.Services.AddScoped<IMovieService, MovieService>();

			builder.Services.AddHttpContextAccessor();

			builder.Services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.Authority = builder
					.Configuration
					.GetSection("isUri").Value;
					opt.TokenValidationParameters.ValidateAudience = false;
					opt.TokenValidationParameters.ValidTypes =
					new[] { "at+jwt" };
				});
		}
	}
}