﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;
using WEB_153503_DAINOVICH.Services.MovieTypeService;
using WEB_153503_DAINOVICH.Services.MovieService;
using WEB_153503_DAINOVICH.Controllers;

namespace WebLab.Tests
{
    public class MovieControllerTests
    {
        [Fact]
        public async Task Index_MovieTypesNotRecieved_ReturnsNotFound()
        {
            var movieTypeServiceMock = new Mock<IMovieTypeService>();
            movieTypeServiceMock.Setup(s => s.GetMovieTypeListAsync())
                .ReturnsAsync(new ResponseData<List<MovieType>>(null) { Success = false, ErrorMessage = "ERROR" });

            var MovieServiceMock = new Mock<IMovieService>();

            var controller = new MovieController(MovieServiceMock.Object, movieTypeServiceMock.Object);

            var result = await controller.Index(null, default);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Index_MoviesNotReceived_ReturnsNotFound()
        {
            var movieTypeServiceMock = new Mock<IMovieTypeService>();
            movieTypeServiceMock.Setup(s => s.GetMovieTypeListAsync())
                .ReturnsAsync(new ResponseData<List<MovieType>>(default));

            var MovieServiceMock = new Mock<IMovieService>();
            MovieServiceMock.Setup(s => s.GetMovieListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Movie>>(null) { Success = false, ErrorMessage = "Error" });

            var controller = new MovieController(MovieServiceMock.Object, movieTypeServiceMock.Object);

            var result = await controller.Index(null, default);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Index_CorrectAction()
        {
            var expectedMovieType = new MovieType { Id = 1, Name = "Movie", NormalizedName = "movie" };
            var expectedMovieTypes = new List<MovieType>() { expectedMovieType };

            var movieTypeServiceMock = new Mock<IMovieTypeService>();
            movieTypeServiceMock.Setup(s => s.GetMovieTypeListAsync())
                .ReturnsAsync(new ResponseData<List<MovieType>>(expectedMovieTypes));

            var wineMocks = new List<Movie>() { new Movie { Id = 1, Title = "Movie", Description = "Description", Type = expectedMovieType, TypeId = expectedMovieType.Id, Price = 10 } };

            var MovieServiceMock = new Mock<IMovieService>();
            MovieServiceMock.Setup(s => s.GetMovieListAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new ResponseData<ListModel<Movie>>(new ListModel<Movie>(wineMocks)));

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.Request.Headers)
                .Returns(new HeaderDictionary { { "X-Requested-With", "XMLHttpRequest" } });

            var controller = new MovieController(MovieServiceMock.Object, movieTypeServiceMock.Object) { ControllerContext = new() { HttpContext = httpContextMock.Object } };

            var result = await controller.Index(expectedMovieType.NormalizedName, default);

            Assert.Equal(expectedMovieTypes, controller.ViewData["movieTypes"]);
            Assert.Equal(expectedMovieType.Name, controller.ViewData["movieType"]);
            Assert.IsType<ListModel<Movie>>(controller.ViewData.Model);
        }
    }
}