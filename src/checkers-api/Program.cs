using System.Text.Json.Serialization;
using checkers_api.Hubs;
using checkers_api.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddSingleton<IRoomManager, RoomManager>();
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();

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
