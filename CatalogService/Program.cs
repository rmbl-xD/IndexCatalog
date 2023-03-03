using Azure.Storage.Blobs;
using CatalogService.Data;
using CatalogService.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database EF
builder.Services.AddDbContext<DataContext>();

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
    EndPoints = { "indexcatalog.redis.cache.windows.net:6380" },
    Password = "8YxMIbahe5bbNs3LcdAv33oldIqNi5lZBAzCaA7RndE=",
    Ssl = true
};

var multiplexer = ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);


//Azure Storage Account
builder.Services.AddScoped(_ => { return new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=indexcataglogstore;AccountKey=CkYWZ8ycpd/Qbc9zkLFFBDIWOUzYjzlCDlWiwBkxYamKdtDLRs64PkM11AbNfJsSRgpXkQXBcFxm+AStq5ZT2A==;EndpointSuffix=core.windows.net"); });
builder.Services.AddScoped<IImageService, ImageService>();

var app = builder.Build();

app.UseOutputCache();

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