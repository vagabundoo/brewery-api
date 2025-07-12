using brewery_api;

var db = new BreweryContext();

// Reset database
db.Database.EnsureDeleted();
db.Database.EnsureCreated();


// Data to prefill database
var beer1 = 


// Create brewery
db.Add(new Brewery
{
    Name = "Leffe",
    Beers = new List<Beer>()
})


var brewery = new Brewery { Name = "Leffe" };
db.Breweries.Add(brewery);
await db.SaveChangesAsync();
            
// Read
var retrieved = await db.Breweries
    .OrderBy(b => b.Id)
    .FirstOrDefaultAsync();


var generator = new MockDataGenerator(db);
await generator.GenerateAsync();