using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLab.API.Data;
using WebLab.API.Services.MovieTypeService;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;

namespace WebLab.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieTypesController : ControllerBase
    {

		private readonly IMovieTypeService _movieTypeService;

		public MovieTypesController(IMovieTypeService movieTypeService)
		{
			_movieTypeService = movieTypeService;
		}

		// GET: api/MovieTypes
		[HttpGet]
		public async Task<ActionResult<ResponseData<MovieType[]>>> GetMovieTypes()
		{
			var movieTypes = await _movieTypeService.GetMovieTypeListAsync();
			return Ok(movieTypes);
		}
	}
}
