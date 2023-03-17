using Azure.Storage.Blobs;
using CatalogService.Caching;
using CatalogService.Data;
using CatalogService.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database EF
builder.Services.AddDbContext<DataContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("CatalogDb"), 
        options => options.EnableRetryOnFailure()));

//eigene Service
builder.Services.AddScoped<IMetaService, MetaService>();
builder.Services.AddScoped<IFranchiseService, FranchiseService>();


//Asp.Net output cache
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(10);
});


//Redis Cache
var options = new ConfigurationOptions
{
    AbortOnConnectFail = false,
    EndPoints = { builder.Configuration["Redis:EndPoint"] ?? string.Empty },
    Password = builder.Configuration["Redis:Password"],
    Ssl = true
};

var multiplexer = ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddSingleton<ICacheService, RedisCacheService>();

//Azure Storage Account
builder.Services.AddScoped(_ => new BlobServiceClient(builder.Configuration["AzureStorage:Connection"]));
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

app.UseOutputCache();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();