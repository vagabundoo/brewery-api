using brewery_api;
using brewery_api.Services;

namespace brewery_unit_tests;

public class WholesalerBuyBeerTests
{
    [Test]
    public async Task BuyBeer()
    {
        var db = new BreweryContext();
        var service = new WholesalerService(db);
        
        var brewery = new Brewery{Name = "Leffe"};
        db.Breweries.Add(brewery);
        await db.SaveChangesAsync();

        var beer = new Beer { Name = "Leffe Blond", BreweryId = brewery.Id };
        db.Beers.Add(beer); 
        await db.SaveChangesAsync();
        
        var wholesaler = new Wholesaler{Name = "BeerMonger"};
        db.Wholesalers.Add(wholesaler);
        await db.SaveChangesAsync();

        var result = await service
            .BuyBeer(wholesaler.Id, beer.Id, 200);
        Assert.That(result, Is.Not.Null);
        
        var beers= result.Beers;
        Assert.That(beers, Has.Count.EqualTo(1));
        var addedBeer = beers.First();
        Assert.That(addedBeer.Amount, Is.EqualTo(200));
    }

    [Test]
    public async Task MissingWholesaler()
    {
        var db = new BreweryContext();
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
        var service = new WholesalerService(db);
        var wholesaler = db.Wholesalers;
        await db.SaveChangesAsync();
        var result = await service
            .BuyBeer(1, 1, 200);
        
        Assert.That(result, Is.Null);
    }
}