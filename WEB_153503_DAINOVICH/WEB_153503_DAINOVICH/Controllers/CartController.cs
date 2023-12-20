using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WEB_153503_DAINOVICH.Services.MovieService;
using WebLab.Domain;

namespace WEB_153503_DAINOVICH.Controllers
{
    public class CartController : Controller
	{
		private readonly IMovieService _movieService;
		private readonly Cart _cart;

		public CartController(IMovieService MovieService, Cart cart)
		{
			_movieService = MovieService;
			_cart = cart;
		}

		[Authorize]
		[Route("[controller]")]
		public ActionResult Index()
		{
			return View(_cart);
		}

		[Authorize]
		[Route("[controller]/add/{id:int}")]
		public async Task<ActionResult> Add(int id, string returnUrl)
		{
			var data = await _movieService.GetMovieByIdAsync(id);
			if (data.Success)
			{
				_cart.AddToCart(data.Data);
			}
			return Redirect(returnUrl);
		}

		[Authorize]
		[Route("[controller]/remove/{id:int}")]
		public async Task<ActionResult> Remove(int id, string returnUrl)
		{
			_cart.RemoveItems(id);
			return Redirect(returnUrl);
		}
	}
}