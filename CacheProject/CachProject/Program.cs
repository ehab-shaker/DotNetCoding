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



app.MapGet("/SearchNames", async (string str, ICache _ICash) =>
{
    return await _ICash.GetOrCreateAsync($"CachedName{str}", Helper.SearchNames, str);

});

app.MapGet("/SearchIds", async (int id, ICache _ICash) =>
{
    return await _ICash.GetOrCreateAsync($"CachedId{id}", Helper.SearchIds, id);
});

app.MapGet("/SearchIdsGeneric", async (int id, ICache _ICash) =>
{
    return  await _ICash.GetOrCreateWithGenericAsync($"CachedId{id}", async id => await Helper.SearchIds(id), id);
});
app.MapGet("/SearchNamesGeneric", async (string str, ICache _ICash) =>
{
    return await _ICash.GetOrCreateWithGenericAsync($"CachedId{str}", async id => await Helper.SearchNames(str), str);
});

app.Run();