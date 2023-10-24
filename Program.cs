using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using twizzle.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });
builder.Services.AddControllers();
builder.Services.AddDbContext<TwizzleDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
        {
            options.Cookie.Name = "TwizzleCookie";
            options.Cookie.HttpOnly = true;
            options.Cookie.MaxAge = TimeSpan.FromMinutes(1);
            options.LoginPath = "/api/auth/login";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
    {
        options
            .AddPolicy(name: MyAllowSpecificOrigins,
                       builder =>
                        {
                            builder
                          .WithOrigins("https://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                        });
    });

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors(MyAllowSpecificOrigins);

app.Run();
