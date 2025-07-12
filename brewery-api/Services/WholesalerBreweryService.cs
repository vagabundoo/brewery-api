namespace brewery_api;

public class WholesalerBreweryService
{
    void BuyBeer(Wholesaler wholesaler, Beer beer)
    {
        var existingBeer = wholesaler.Beers.FirstOrDefault(b => b.Id == beer.Id);

        if (existingBeer != null)
        {
            existingBeer.Amount += beer.Amount;
        }
        else
        {
            wholesaler.Beers.Add(beer);
        }
    }
}