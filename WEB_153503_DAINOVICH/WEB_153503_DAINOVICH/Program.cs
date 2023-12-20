using Microsoft.IdentityModel.Logging;
using Serilog;
using WEB_153503_DAINOVICH.Middleware;
using WEB_153503_DAINOVICH.Models;
using WEB_153503_DAINOVICH.Services.CartService;
using WEB_153503_DAINOVICH.Services.MovieService;
using WEB_153503_DAINOVICH.Services.MovieTypeService;
using WEB_153503_DAINOVICH.TagHelpers;
using WebLab.Domain;
using WebLab.Services.MovieService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IMovieTypeService, ApiMovieTypeService>();
builder.Services.AddScoped<IMovieService, ApiMovieService>();

builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));

builder.Services.AddScoped<PagerTagHelper>();

var uriData = builder.Configuration.GetSection("UriData").Get<UriData>()!;
builder.Services.AddHttpClient("API", opt => opt.BaseAddress = new(uriData.ApiUri));

var logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

builder.Services.AddHttpContextAccessor();
builder.Services
	.AddAuthentication(opt =>
	{
		opt.DefaultScheme = "access_token";
		opt.DefaultChallengeScheme = "oidc";
	})
	.AddCookie("access_token")
	.AddOpenIdConnect("oidc", options =>
	{
		options.Authority = builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
		options.ClientId = builder.Configuration["InteractiveServiceSettings:ClientId"];
		options.ClientSecret = builder.Configuration["InteractiveServiceSettings:ClientSecret"];
		options.GetClaimsFromUserInfoEndpoint = true;
		options.ResponseType = "code";
		options.ResponseMode = "query";
		options.SaveTokens = true;
	});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//IdentityModelEventSource.ShowPII = true;
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

/*app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapRazorPages().RequireAuthorization();

app.UseMiddleware<LoggingMiddleware>(logger);

app.Run();
