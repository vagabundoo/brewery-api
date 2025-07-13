using brewery_api;
using brewery_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Scalar.AspNetCore;

var db = new BreweryContext();

// Reset database
//Database logic
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

InitBreweries(db);
var beers = await db.Beers
    .ToListAsync();
InitWholesalers(db, beers);

var wholesaler = await db.Wholesalers.FirstAsync();

wholesaler.Beers
    .ForEach(b => Console.WriteLine($"{b.Id} Beer: {b.Name}, Amount: {b.Amount}"));

var beerToPurchase = beers[0];
wholesaler.Beers
    .Find(b => b.Id == beerToPurchase.Id)
    .Amount += 20;

beerToPurchase = db.Beers.Find(beerToPurchase.Id);
WholesalerBreweryService.IncrementBeerAmount(beerToPurchase, 50);
await db.SaveChangesAsync();

wholesaler.Beers
    .ForEach(b => Console.WriteLine($"{b.Id} Beer: {b.Name}, Amount: {b.Amount}"));

// Client asks for quote
var beerOrders = new List<BeerOrder>
{
    new BeerOrder("Leffe Blond", 30),
};

var service = new ClientWholesalerService();
var errorMessage = service.GetReasonOrderInvalid(beerOrders, wholesaler);

beerOrders = service.AddPriceToBeerOrders(beerOrders, beers);

var totalPrice = service.GetTotalPriceWithDiscount(beerOrders);

var beerOrderSummary = service.GetQoute(beerOrders, wholesaler, wholesaler.Beers);

// Api Logic
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BreweryContext>();

var app = builder.Build();


app.MapGet("/beer", async (BreweryContext db) =>
    await db.Beers.ToListAsync());

app.MapGet("/beer/id={id}", async (BreweryContext db, int id) =>
    await db.Beers
        .Where(b => b.Id == id)
        .ToListAsync());

app.MapPatch("/beer/id={id}price={price}", async (BreweryContext db, int id, double price) =>
{
    var beer = db.Beers
        .FirstOrDefault(b => b.Id == id);
    if (beer == null)
    {
        return Results.NotFound();
    }

    beer.Price = price;
    await db.SaveChangesAsync();
    return Results.Ok(beer);
});

app.MapDelete("/beer/{id}", async (BreweryContext db, int id) =>
{
    var beer = db.Beers.FirstOrDefault(b => b.Id == id);
    if (beer == null)
    {
        return Results.NotFound();
    }

    db.Beers.Remove(beer);
    await db.SaveChangesAsync();
    return Results.Ok($"{beer.Name} has been deleted from database");
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

app.MapGet("/quote/sample", () =>
{
    //var quote = new { Summary = summary, Price = price };
    //return Results.Json(quote);
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
    //db.Wholesa
}


//var beers = db.Beers.Local.ToHashSet();

