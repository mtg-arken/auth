using Microsoft.AspNetCore.Authentication.OAuth;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddAuthentication(config =>
{
	// We check the cookie to confirm that we are authenticated
	config.DefaultAuthenticateScheme = "ClientCookie";
	// When we sign in we will deal out a cookie
	config.DefaultSignInScheme = "ClientCookie";
	// use this to check if we are allowed to do something.
	config.DefaultChallengeScheme = "OurServer";
})
.AddCookie("ClientCookie")
.AddOAuth("OurServer", config =>
{
	config.ClientId = "your_client_secret";
	config.ClientSecret = "your_client_secret";
	config.CallbackPath = "/oauth/callback";
	config.AuthorizationEndpoint = "https://localhost:7126/oauth/login";
	config.TokenEndpoint = "https://localhost:7126/oauth/token";
	config.SaveTokens = true;

	config.Events = new OAuthEvents()
	{
		OnCreatingTicket = context =>
		{
			var accessToken = context.AccessToken;
			var base64payload = accessToken.Split('.')[1];
			var bytes = Convert.FromBase64String(base64payload);
			var jsonPayload = Encoding.UTF8.GetString(bytes);
			var claims = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonPayload);

			foreach (var claim in claims)
			{
				context.Identity.AddClaim(new Claim(claim.Key, claim.Value));
			}

			return Task.CompletedTask;
		}
	};
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
