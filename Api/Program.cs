using Api;
using Api.Data;
using Api.IRepository;
using Api.Profile;
using Api.Repository;
using Api.Services;
using AutoMapper;
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
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookListConnectionString"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerAuthenticationBearer();
builder.Services.ConfigureIdentityServices(builder.Configuration);

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowAllocationRepository, BorrowAllocationRepository>();
builder.Services.AddScoped<IBorrowRequestRepository, BorrowRequestRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();







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
