using brewery_api;
using Microsoft.EntityFrameworkCore;

var db = new BreweryContext();

// Reset database
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

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
db.Beers.Attach(beer1);
db.Beers.Attach(beer2);
await db.SaveChangesAsync();

var wholesaler = new Wholesaler
{
    Name = "BeersRwe",
    Beers = new List<Beer> { beer1, beer2 },
};

wholesaler.Beers.ForEach(b => b.Amount = 0);
db.Wholesalers.Attach(wholesaler);
await db.SaveChangesAsync();

wholesaler.Beers
    .FindAll(b => b.Id == beer1.Id)
    .ForEach(b => Console.WriteLine($"Beer 1: {b.Name}, Amount: {b.Amount}"));


wholesaler.Beers.Find(b => b.Name == beer1.Name)!.Amount += 10;
await db.SaveChangesAsync();

wholesaler.Beers
    .FindAll(b => b.Id == beer1.Id)
    .ForEach(b => Console.WriteLine($"Beer 1: {b.Name}, Amount: {b.Amount}"));

