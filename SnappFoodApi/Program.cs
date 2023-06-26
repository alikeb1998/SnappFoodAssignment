using DataAccess;
using Domain.Repositories;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//     .AddNegotiate();
//
// builder.Services.AddAuthorization(options =>
// {
//     // By default, all incoming requests will be authorized according to the default policy.
//     options.FallbackPolicy = options.DefaultPolicy;
// });
var dbConnection = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<SnappFoodDbContext>(options =>
{
    options.UseSqlServer(dbConnection,
        sqlServerOptionsAction: sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 10,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICacheProvider, InMemoryCacheProvider>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddMemoryCache();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();