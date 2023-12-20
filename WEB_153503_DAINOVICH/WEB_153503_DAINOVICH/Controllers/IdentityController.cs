using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEB_153503_DAINOVICH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
                "oidc",
                new AuthenticationProperties
                {
                    RedirectUri =
                Url.Action("Index", "Home")
                });
        }

		[HttpPost]
		public async Task Logout()
        {
            await HttpContext.SignOutAsync("access_token");
            await HttpContext.SignOutAsync("oidc",
            new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
        }

    }
}
