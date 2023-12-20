using Microsoft.EntityFrameworkCore;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;

namespace WebLab.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();


            var action = new MovieType
            {
                Name = "Боевики", 
                NormalizedName = "action"
            };
            var drama = new MovieType
            {
                Name = "Драмы",
                NormalizedName = "drama"
            };
            var anime = new MovieType
            {
                Name = "Аниме",
                NormalizedName = "anime"
            };
            var movieTypes = new List<MovieType>() { action, drama, anime };

            await dbContext.MovieType.AddRangeAsync(movieTypes);

            var applicationUrl = app.Configuration.GetValue<string>("ApplicationUrl");

            await dbContext.SaveChangesAsync();

            var movies = new List<Movie> {
                new Movie()
                {
                    Title = "Криминальное чтиво",
                    Description = "Описание фильма 1",
                    Price = 200,
                    Image = "Images/pulp-fict.png",
                    Type = action,
                    TypeId = action.Id
                },
                new Movie {
                    Title = "Леон",
                    Description = "Описание фильма 2",
                    Price = 220,
                    Image = "Images/leon.png",
                    Type = action,
                    TypeId = action.Id},

                new Movie {
                    Title = "Бегущий по лезвию 2049",
                    Description = "Описание фильма 3",
                    Price = 180,
                    Image = "Images/blade-runner.png",
                    Type = drama,
                    TypeId = drama.Id},

                new Movie { 
                    Title = "Крутой учитель Онидзука",
                    Description = "Описание фильма 4",
                    Price = 110,
                    Image = "Images/gto.png",
                    Type = anime,
                    TypeId = anime.Id},

                 new Movie { 
                     
                     Title = "Человек-Бензопила",
                    Description = "Описание фильма 5",
                    Price = 105,
                    Image = "Images/chainsaw-man.png",
                    Type = anime,
                    TypeId = anime.Id},

                new Movie { 
                    Title = "Бойцовский клуб",
                    Description = "Описание фильма 6",
                    Price = 150,
                    Image = "Images/fight-club.png",
                    Type = drama,
                    TypeId = drama.Id},

                 new Movie { 
                    Title = "La Haine",
                    Description = "Описание фильма 7",
                    Price = 115,
                    Image = "Images/la-haine.png",
                    Type = drama,
                    TypeId = drama.Id},

                new Movie { 
                    Title = "Джон Уик",
                    Description = "Описание фильма 8",
                    Price = 170,
                    Image = "Images/john-wick.png",
                    Type = action,
                    TypeId = action.Id}
            };

            await dbContext.Movie.AddRangeAsync(movies);
            await dbContext.SaveChangesAsync();
        }
    }
}
