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

// Burada scoped dedik çünkü; IProductRespository'i görürsen git ProductRepository'den bir nesne örneði al diyoruz. Burada scope dedik çünkü her çalýþtýðýmýzda tek bir nesne örneði kullanýlsýn, eðer request response'a dönerse bu nesne örneði kaldýrýlsýn diye kullandýk.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // EnsureCreated metodunu in-memory bir db kullandýðýmýz için çaðýrmamýz gerekiyor. Bunu yapýyoruz ki datalarýmýzý çekebilelim. Buradaki dbcontext scope bitene kadar ayakta kalýr, scope bittiðinde de context tamamlanýr ve kaldýrýlýr.
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
