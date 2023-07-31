using JWTDemo.DataContext;
using JWTDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<DemoTokenContext>(options =>
{
	options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddTransient<IdentityService>();
string key = configuration.GetSection("ServiceConfiguration").GetSection("JwtSettings").GetSection("Secret").Value;
var tokenValidationParameters = new TokenValidationParameters
{
	ValidateIssuer = false,
	ValidateAudience = false,
	ValidateIssuerSigningKey = true,
	IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
};


builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer();
// This is the tricky part to inject the configuration so the public key is ued to validate the JWT
}

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
