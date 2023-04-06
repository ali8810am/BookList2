using Api;
using Api.Data;
using Api.IRepository;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(config =>
{
    config.CacheProfiles.Add("120SecondDuration", new CacheProfile
    {
        Duration = 120
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookListConnectionString"));
});

builder.Services.ConfigureSwaggerAuthenticationBearer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureIdentityServices(builder.Configuration);

builder.Services.AddScoped<IAuthManager, AuthManager>();
//builder.Services.AddTransient<IUnitOfWork, IUnitOfWork>();


var app = builder.Build();
app.UseAuthentication();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
