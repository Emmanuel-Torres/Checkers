using System.Text.Json.Serialization;
using Amazon.S3;
using checkers_api.Hubs;
using checkers_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var clientId = builder.Configuration["GOOGLE-CLIENT-ID"];
// Add services to the container.

builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IMatchmakingService, MatchmakingService>();
builder.Services.AddSingleton<IImageService, ImageService>();
builder.Services.AddTransient<IUserDbService, UserDbService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSignalR().AddJsonProtocol(options =>
    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer(options =>
{
    options.Audience = clientId;
    options.Authority = "https://accounts.google.com";
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = clientId,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidIssuers = new List<string>() { "https://accounts.google.com", "accounts.google.com" }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<CheckersHub>("/hubs/checkers");
app.MapControllers();

app.Run();
