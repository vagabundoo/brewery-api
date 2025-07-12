namespace brewery_api.Services;

public class ClientWholesalerService
{
    public (List<BeerOrder>, double) GetQoute(List<BeerOrder> beerOrders, Wholesaler wholesaler, List<Beer> beers)
    {
        var invalidOrderMessage = GetReasonOrderInvalid(beerOrders, wholesaler);
        if (invalidOrderMessage != null)
        {
            Console.WriteLine(invalidOrderMessage);
        }
        
        beerOrders = AddPriceToBeerOrders(beerOrders, beers);
        var totalPrice = GetTotalPriceWithDiscount(beerOrders);
        
        return (beerOrders, totalPrice);
    }

    public double GetTotalPriceWithDiscount(List<BeerOrder> beerOrders)
    {
        int amountOrdered = (from order in beerOrders select order.BeerAmount).Sum();
        double totalPrice = (from order in beerOrders select order.TotalPrice).Sum() ?? 0;

        if (amountOrdered >= 20)
        {
            totalPrice *= 0.80;
        }
        else if (amountOrdered >= 10)
        {
            totalPrice *= 0.90;
        }


        return totalPrice;

    }

    public string? GetReasonOrderInvalid(List<BeerOrder> beerOrders, Wholesaler? wholesaler)
    {
        if (beerOrders.Count == 0)
        {
            return "order is empty";    
        }
        if (wholesaler == null)
        {
            return "wholesaler must exist";
        }
        
        var allUnique = beerOrders.Distinct();
        if (allUnique.Count() != beerOrders.Count)
        {
            return "there can't be any duplicates in the order";
        }

        foreach (var beerOrder in beerOrders)
        {
            var availableBeers = wholesaler.Beers;
            var beerAvaliable = availableBeers.Any(b => b.Name == beerOrder.BeerName);
            if (!beerAvaliable)
            {
                return "the beer must be sold by the wholesaler";
            }

            var beerInStock = (
                from b in availableBeers 
                where b.Name == beerOrder.BeerName 
                && b.Amount >= beerOrder.BeerAmount
                select 1).Any();

            if (!beerInStock)
            {
                return "the number of beers ordered cannot be greater than the wholesaler's stock";
            }
        }
        return null;
    }
    
    public List<BeerOrder> AddPriceToBeerOrders(List<BeerOrder> beerOrders, List<Beer> beers)
    {
        foreach (var beerOrder in beerOrders)
        {
            var beerName = beerOrder.BeerName;
            var price = beers.FirstOrDefault(b => b.Name == beerName)?.Price ?? 0;
            beerOrder.TotalPrice = price;
        }
        return beerOrders;
    }
}

public class BeerOrder(string beerName, int beerAmount)
{
    public readonly string BeerName = beerName;
    public readonly int BeerAmount = beerAmount;
    public double? TotalPrice = null;
}