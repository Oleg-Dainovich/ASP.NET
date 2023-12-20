using System.Text.Json.Serialization;
using WEB_153503_DAINOVICH.Extensions;
using WebLab.Domain.Entities;

namespace WEB_153503_DAINOVICH.Services.CartService
{
	public class SessionCart : WebLab.Domain.Cart
	{
		[JsonIgnore]
		private ISession? _session;

		public static WebLab.Domain.Cart GetCart(IServiceProvider services)
		{
			ISession? session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
			SessionCart cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
			cart._session = session;
			return cart;
		}

		public override void AddToCart(Movie movie)
		{
			base.AddToCart(movie);
			_session.Set("Cart", this);
		}

		public override void RemoveItems(int id)
		{
			base.RemoveItems(id);
			_session.Set("Cart", this);
		}

		public override void ClearAll()
		{
			base.ClearAll();
			_session.Remove("Cart");
		}

	}
}