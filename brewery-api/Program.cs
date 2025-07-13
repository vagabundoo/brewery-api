using brewery_api;
using brewery_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var breweryDb = new BreweryContext();

// In order to avoid having to deal with migrations and database state,
// we delete and create the database every time project is run.
breweryDb.Database.EnsureDeleted();
breweryDb.Database.EnsureCreated();

// Prefilling using Db methods.
var breweryService = new BreweryService(breweryDb);
await breweryService.Create("Leffe");
await breweryService.Create("Duvel");

var beerService = new BeerService(breweryDb);
await beerService.Create(1, "Leffe Blond", 4);
await beerService.Create(2, "Duvel Blond", 5);

var wholesalerService = new WholesalerService(breweryDb);
await wholesalerService.Create("LocalBrew");


// Api Logic
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BreweryContext>();
builder.Services.AddScoped<BeerService>();
builder.Services.AddScoped<BreweryService>();
builder.Services.AddScoped<WholesalerService>();

var app = builder.Build();

app.MapGet("/beer", async (BeerService beerService) =>
{
    var beers = await beerService.GetAllAsync();
    return Results.Ok(beers);
});

app.MapGet("/beer/{id:int}", async (int id, BeerService beerService) =>
    {
        var beer = await beerService.GetByIdAsync(id);
        return beer is null 
            ? Results.NotFound() 
            : Results.Ok(beer);
    });

app.MapPost("/beer/{breweryId:int}/{name}/{price:double}", 
    async (int breweryId, string name, double price, BeerService beerService) =>
{
    var beer = await beerService.Create(breweryId, name, price);
    return beer is null
        ? Results.NotFound()
        : Results.Ok(beer);
});

app.MapPatch("/beer/{id:int}/price/{price:double}", async (int id, double price, BeerService beerService) =>
{
    var beer = await beerService.UpdatePriceAsync(id, price);
    return beer is null 
        ? Results.NotFound() 
        : Results.Ok(beer);
});

app.MapDelete("/beer/{id}", async (BeerService service, int id) =>
{
    var beer = await service.DeleteAsync(id);
    return beer is null
        ? Results.NotFound() 
        : Results.Ok($"{beer.Name} has been deleted from database");
});

app.MapGet("/brewery", async (BreweryService breweryService) =>
{
    var breweries = await breweryService.GetAllAsync();
    return Results.Ok(breweries);
});;

app.MapGet("/brewery/{id}", async (int id, BreweryService breweryService) =>
{
    var brewery = await breweryService.GetByIdAsync(id);
    return brewery is null
        ? Results.NotFound()
        : Results.Ok(brewery);
});;

app.MapGet("/wholesaler", async (WholesalerService wholesalerService) =>
{
    var wholesalers = await wholesalerService.GetAllAsync();
    return Results.Ok(wholesalers);
});;

app.MapGet("/wholesaler/{id}", async (int id, WholesalerService wholesalerService) =>
{
    var wholesaler = await wholesalerService.GetByIdAsync(id);
    return wholesaler is null
        ? Results.NotFound()
        : Results.Ok(wholesaler);
});;
/*
app.MapPost("/wholesaler/{wholesalerId}/beer/{beerId}", 
    async (BreweryContext db, int wholesalerId, int beerId) =>
{
    var wholesaler = db.Wholesalers
        .Include(wholesaler => wholesaler.Beers)
        .FirstOrDefault(w => w.Id == wholesalerId);
    if (wholesaler == null)
    {
        return Results.NotFound();
    }
    var beer = db.Beers.FirstOrDefault(b => b.Id == beerId);
    if (beer == null)
    {
        return Results.NotFound();
    }
    var wholesalerBeers = wholesaler.Beers;
    wholesalerBeers.Add(beer);
    await db.SaveChangesAsync();
    return Results.Ok($"Beer has been added to {wholesaler.Name}.\n Name: {beer.Name}, Price: {beer.Price}, BreweryId: {beer.BreweryId}");
});

app.MapGet("/quote/sample", () =>
{
});

app.MapPost("/quote/wholesalerName={wholesalerName}&beerName={beerName}&beerAmount={beerAmount}", 
    async (BreweryContext db, string wholesalerName, string beerName, int beerAmount) =>
{
    var beerOrders = new List<BeerOrder>
    {
        new BeerOrder(
           beerName,
           beerAmount
        ), 
    };
   var wholesaler = await db.Wholesalers
       .FirstOrDefaultAsync(w => w.Name == wholesalerName);
   
   var availableBeers = await db.Beers
       .Where(b => b.Name == beerName)
       .ToListAsync();
    
    var service = new ClientService();
    var quote = service.GetQuote(beerOrders, wholesaler, availableBeers);
    
    return Results.Ok($"{quote.TextSummary}");
});
*/

app.Run();
return;




