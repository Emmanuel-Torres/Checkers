using System.Text.Json.Serialization;
using checkers_api.Hubs;
using checkers_api.Services;
using checkers_api.Services.GameManager;
using checkers_api.Services.Matchmaking;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

// builder.Services.AddSingleton<IGameManager, GameManager>();
builder.Services.AddSingleton<IMatchmakingService, MatchmakingService>();
// builder.Services.AddTransient<IDbService, DbService>();
// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSignalR().AddJsonProtocol(options =>
   options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.MapControllers();
app.MapHub<CheckersHub>("/hubs/checkers");

app.Run();
