using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("myDatabase");
});

// Burada scoped dedik ��nk�; IProductRespository'i g�r�rsen git ProductRepository'den bir nesne �rne�i al diyoruz. Burada scope dedik ��nk� her �al��t���m�zda tek bir nesne �rne�i kullan�ls�n, e�er request response'a d�nerse bu nesne �rne�i kald�r�ls�n diye kulland�k.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // EnsureCreated metodunu in-memory bir db kulland���m�z i�in �a��rmam�z gerekiyor. Bunu yap�yoruz ki datalar�m�z� �ekebilelim. Buradaki dbcontext scope bitene kadar ayakta kal�r, scope bitti�inde de context tamamlan�r ve kald�r�l�r.
    dbContext.Database.EnsureCreated();
}

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
