using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC ve API'yi ayný anda ekliyoruz
builder.Services.AddControllersWithViews();  // MVC için
builder.Services.AddControllers(); // API için

// Swagger'ý sadece API için etkinleþtirin
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Oturum süresi (30 dakika)
    options.Cookie.HttpOnly = true; // Çerezlere sadece sunucu tarafýndan eriþilebilmesi için
    options.Cookie.IsEssential = true; // Çerezlerin GDPR uyumlu olmasý için gerekli
});

builder.Services.AddDbContext<RepositoryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Swagger'ý sadece API uç noktalarý için etkinleþtirme
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();


// MVC route ekleyin (login sayfasýna yönlendirme)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LoginUser}/{id?}"); // Login sayfasýna yönlendirme

// API route
app.MapControllers(); // API uç noktalarý için

app.Run();
