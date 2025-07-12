namespace brewery_api;

public static class WholesalerBreweryService
{
    public static Wholesaler BuyBeer(Wholesaler wholesaler, Beer beer, int amount)
    {
        var existingBeer = wholesaler.Beers.FirstOrDefault(b => b.Name == beer.Name);

        if (existingBeer != null)
        {
            existingBeer.Amount += amount;
        }
        else
        {
            wholesaler.Beers.Add(beer);
        }
        return wholesaler;
    }
}