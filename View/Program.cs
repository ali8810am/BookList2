
using View.Contracts;
using View.Services;
using View.Services.Base;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Assembly = System.Reflection.Assembly;
using PathString = Microsoft.AspNetCore.Http.PathString;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpContextAccessor();


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/user/login");
}); ;
builder.Services.AddRazorPages();
builder.Services.AddHttpClient<IClient,Client>("MyWebApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7043");
});
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());


builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBorrowAllocationService, BorrowAllocationService>();
builder.Services.AddScoped<IBorrowRequestService, BorrowRequestService>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
//builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
