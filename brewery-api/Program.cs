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

InitBreweries(breweryDb);
var beers = await breweryDb.Beers
    .ToListAsync();
InitWholesalers(breweryDb, beers);

// Api Logic
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BreweryContext>();
builder.Services.AddScoped<BeerService>();

var app = builder.Build();

app.MapGet("/beer", async (BeerService beerService) =>
{
    var foundBeers = await beerService.GetAllAsync();
    return Results.Ok(foundBeers);
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

app.MapGet("/brewery", async (BreweryContext db) => 
    await db.Breweries.ToListAsync());

app.MapGet("/brewery/{breweryId}",
    async (BreweryContext db, int breweryId) =>
    {
        var brewery = db.Breweries
            .FirstOrDefault(br => br.Id == breweryId);
        if (brewery == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(brewery);
    });

app.MapPost("/brewery/{breweryId}/beer/name={name}price={price}",
    async (BreweryContext db, int breweryId, string name, double price) =>
    {
        var brewery = db.Breweries
            .FirstOrDefault(br => br.Id == breweryId);
        if (brewery == null)
        {
            return Results.NotFound();
        }
        
        var beer = new Beer
        {
            Name = name,
            Price = price,
            BreweryId = brewery!.Id
        };
        
        db.Beers.Add(beer);
        await db.SaveChangesAsync();

        return Results.Ok($"Beer has been added. Name: {beer.Name}, Price: {beer.Price}, BreweryId: {beer.BreweryId}");
    });

app.MapGet("/wholesaler", async (BreweryContext db) =>
    await db.Wholesalers.ToListAsync());

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
    
    var service = new ClientWholesalerService();
    var quote = service.GetQuote(beerOrders, wholesaler, availableBeers);
    
    return Results.Ok($"{quote.TextSummary}");
});

app.Run();
return;


async void InitBreweries(DbContext db)
{
    var brewery1 = new Brewery
    {
        Name = "Leffe"
    };
    db.Add(brewery1);
    await db.SaveChangesAsync();
    
    var beer1 = new Beer
    {
        Name = "Leffe Blond",
        BreweryId = brewery1.Id,
        Price = 3,
    };
    var beer2 = new Beer
    {
        Name = "Leffe Tripel",
        BreweryId = brewery1.Id,
        Price = 5,
    };
    db.Add(beer1);
    db.Add(beer2);
    await db.SaveChangesAsync();
}

async void InitWholesalers(DbContext db, List<Beer> beers)
{
    var wholesaler = new Wholesaler
    {
        Name = "BeersRwe",
        Beers = beers,
    };

    wholesaler.Beers.ForEach(b => b.Amount = 0);
    db.Add(wholesaler);
    await db.SaveChangesAsync();
}

async void UpdateWholesalerStock(DbContext db, int amount)
{
    
}




