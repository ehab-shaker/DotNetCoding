using CacheProject;
using CachProject.CashService;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ICache, CacheService>();
builder.Services.Configure<CacheSettings>(builder.Configuration.GetSection("CacheSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/SearchByCategory", async (int category, ICache _ICash) =>
{
    return await _ICash.GetOrCreateAsync($"CachedCategory{category}", Helper.Search, category);
});
app.MapGet("/SearchByType", async (string type, ICache _ICash) =>
{
    return await _ICash.GetOrCreateAsync($"CachedType{type}",/* async id => await */Helper.Search/*(type)*/, type);
});

app.MapGet("/Test",  (ICache _ICash) =>
{
    List<int> items = new();
    Parallel.ForEach(Enumerable.Range(1, 10),
         async i =>
        {
            var item = await _ICash.GetOrCreateAsync("KEY", (x) => Task.FromResult(x), i);
            Console.Write($"{item} ");
        });
});

app.Run();