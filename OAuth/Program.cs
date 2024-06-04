using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication("OAuth").AddJwtBearer("OAuth", options =>
{
	var secretBytes = Encoding.UTF8.GetBytes("not_too_short_secret_otherwise_it_might_error");
	var key = new SymmetricSecurityKey(secretBytes);

	options.Events = new JwtBearerEvents
	{
		OnMessageReceived = context =>
		{
			if (context.Request.Query.ContainsKey("access_token"))
			{
				context.Token = context.Request.Query["access_token"];
			}

			return Task.CompletedTask;
		}
	};

	options.TokenValidationParameters = new TokenValidationParameters
	{
		ClockSkew = TimeSpan.Zero,
		ValidIssuer = "https://localhost:7126",
		ValidAudience = "https://localhost:7291/",
		IssuerSigningKey = key
	};
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
