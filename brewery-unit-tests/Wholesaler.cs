using brewery_api;
using brewery_api.Services;

namespace brewery_unit_tests;

public class WholesalerTests
{
    [Test]
    public async Task BuyBeer()
    {
        var db = new BreweryContext();
        var service = new WholesalerService(db);
        db.Breweries.Add(new Brewery { Name = "Leffe" });
        db.Beers.Add(new Beer{ Name = "Beer", BreweryId = 1}); 
        db.Wholesalers.Add(new Wholesaler {Name = "Seller" });
        await db.SaveChangesAsync();

        var wholesaler = await service
            .BuyBeer(1, 1, 200);
        Assert.That(wholesaler, Is.Not.Null);
        var beers= wholesaler.Beers;
        Assert.That(beers, Has.Count.EqualTo(1));
        var addedBeer = beers.First();
        Assert.That(addedBeer.Amount, Is.EqualTo(200));
    }
}