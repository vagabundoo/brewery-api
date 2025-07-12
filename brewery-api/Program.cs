using brewery_api;
using brewery_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

var (filledOrders, price) = service.GetQoute(beerOrders, wholesaler, wholesaler.Beers);
var summary = service.SummarizeQuoteToString(filledOrders);

Console.WriteLine();

// Api Logic
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BreweryContext>();
var app = builder.Build();

app.MapGet("/beer", async (BreweryContext db) =>
    await db.Beers.ToListAsync());

app.MapGet("/brewery", async (BreweryContext db) => 
    await db.Beers.ToListAsync());

app.MapGet("/wholesaler", async (BreweryContext db) =>
    await db.Wholesalers.ToListAsync());

app.MapGet("/quote/sample", () =>
{
    var quote = new { Summary = summary, Price = price };
    return Results.Json(quote);
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

