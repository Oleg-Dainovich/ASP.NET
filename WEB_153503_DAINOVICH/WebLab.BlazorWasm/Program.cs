using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebLab.BlazorWasm;
using WebLab.BlazorWasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetSection("ApiUri").Value!) });

builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddOidcAuthentication(options =>
{
	// Configure your authentication provider options here.
	// For more information, see https://aka.ms/blazor-standalone-auth
	builder.Configuration.Bind("Local", options.ProviderOptions);
});

await builder.Build().RunAsync();
