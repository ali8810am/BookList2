<<<<<<< HEAD

using BookList.Infrastructure.Data;
=======
using Api;
using Api.Data;
using Api.IRepository;
using Api.Middlewares;
using Api.Profile;
using Api.Repository;
using Api.Services;
using AspNetCoreRateLimit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

<<<<<<< HEAD
builder.Services.AddControllers();
=======
builder.Services.AddControllers(config =>
{
    config.CacheProfiles.Add("120SecondDuration", new CacheProfile
    {
        Duration = 120
    });
})
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
); 

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookListConnectionString"));
});


>>>>>>> e31b9b8125159a0d7956dae5eec28b0187a1cf00
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.ConfigureRateLimiting(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSwaggerAuthenticationBearer();
builder.Services.ConfigureIdentityServices(builder.Configuration);

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBorrowAllocationRepository, BorrowAllocationRepository>();
builder.Services.AddScoped<IBorrowRequestRepository, BorrowRequestRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();







var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseResponseCaching();
app.UseHttpCacheHeaders();
app.UseIpRateLimiting();

app.UseAuthorization();

app.MapControllers();

app.Run();
