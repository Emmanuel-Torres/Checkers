using checkers_api.Data;
using checkers_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var clientId = "203576300472-3j2eeg1m35ahrg4ar8srm36ul8d504h5.apps.googleusercontent.com";
// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("ApplicationContext"))
);

builder.Services.AddTransient<IDbService, DbService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie()
.AddJwtBearer(options => {
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
    // context.Database.EnsureCreated();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
