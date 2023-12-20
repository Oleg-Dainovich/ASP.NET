using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLab.API.Services.MovieService;
using WebLab.Domain.Entities;
using WebLab.Domain.Models;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService MovieService)
    {
        _movieService = MovieService;
    }

    // GET: api/Movies
    [HttpGet("page{pageNo}")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ResponseData<List<Movie>>>> GetMovies(string? movieType, int pageNo = 1, int pageSize = 3)
    {
        var movieList = await _movieService.GetMovieListAsync(movieType, pageNo, pageSize);
        return Ok(movieList);
    }

    // GET: api/Movies/5
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ResponseData<Movie>>> GetMovie(int id)
    {
        var movieResponse = await _movieService.GetMovieByIdAsync(id);
        return Ok(movieResponse);
    }

    // PUT: api/Movies/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> PutMovie(int id, Movie movie)
    {
        await _movieService.UpdateMovieAsync(id, movie);
        return NoContent();
    }

    // POST: api/Movies
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
	[Authorize]
	public async Task<ActionResult<ResponseData<Movie>>> PostMovie(Movie movie)
    {
        var createdMovie = await _movieService.CreateMovieAsync(movie);
        return CreatedAtAction("GetMovie", new { id = createdMovie.Data.Id }, createdMovie);
    }

    // DELETE: api/Movies/5
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        await _movieService.DeleteMovieAsync(id);
        return NoContent();
    }

    // POST: api/Movies/5
    [HttpPost("{id}")]
    [Authorize]
    public async Task<ActionResult<ResponseData<string>>> PostImage(int id, IFormFile formFile)
    {
        var response = await _movieService.SaveImageAsync(id, formFile);
        if (response.Success)
        {
            return Ok(response);
        }
        return NotFound(response);
    }
}