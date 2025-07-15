namespace brewery_api.Services;
/*
public class ClientService
{
    public BeerOrderSummary GetQuote(List<BeerOrder> beerOrders, Wholesaler? wholesaler, List<Beer> beers)
    {
        var summary = new BeerOrderSummary(beerOrders);
        var invalidOrderMessage = GetReasonOrderInvalid(beerOrders, wholesaler);
        if (invalidOrderMessage != null)
        {
            summary.ReasonOrderInvalid = invalidOrderMessage;
            return summary;
        }
        
        beerOrders = AddPriceToBeerOrders(beerOrders, beers);
        summary.TotalPrice = GetTotalPriceWithDiscount(beerOrders);
        summary.TextSummary = SummarizeQuoteToString(beerOrders);
        
        return summary;
    }

    public string SummarizeQuoteToString(List<BeerOrder> beerOrders)
    {
        string result = string.Join("\n", beerOrders
            .Select(b => $"Beer: {b.BeerName}, Amount: {b.BeerAmount}, Price = {b.TotalPrice}"));
        
        return result;
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
            return "Order is empty";    
        }
        if (wholesaler == null)
        {
            return "Wholesaler must exist";
        }
        
        var uniqueBeerNames = beerOrders.Select(b => b.BeerName).Distinct().ToList();
        if (uniqueBeerNames.Count != beerOrders.Count)
        {
            return "There can't be any duplicates in the order";
        }

        foreach (var beerOrder in beerOrders)
        {
            var availableBeers = wholesaler.Beers;
            var beerAvaliable = availableBeers.Any(b => b.Name == beerOrder.BeerName);
            if (!beerAvaliable)
            {
                return "The beer must be sold by the wholesaler";
            }

            var beerInStock = (
                from b in availableBeers 
                where b.Name == beerOrder.BeerName 
                && b.Amount >= beerOrder.BeerAmount
                select 1).Any();

            if (!beerInStock)
            {
                return "The number of beers ordered cannot be greater than the wholesaler's stock";
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

public class BeerOrderSummary(List<BeerOrder> beerOrders)
{
    public List<BeerOrder> BeerOrders = beerOrders;
    public double TotalPrice { get; set; }
    public string? ReasonOrderInvalid { get; set; }
    public string? TextSummary { get; set; }
}*/