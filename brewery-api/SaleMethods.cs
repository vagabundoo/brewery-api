namespace brewery_api;

public class SaleMethods
{
    // Make a qoute
    // requires a dict of beernames, and amounts
    
    // Check if a quote can be met by a brewery or wholesaler
    bool PossibleOrder(List<BeerOrder> beerOrders, List<BeerOrder> beerInventory)
    {
        int requiredOrders = beerOrders.Count();
        int possibleOrders = 0;
        
        foreach (var beerOrder in beerOrders)
        {
            var possibleOrder = beerInventory
                .Where(beerInventory => beerInventory.BeerName == beerOrder.BeerName)
                .Where(beerInventory => beerInventory.BeerAmount >= beerOrder.BeerAmount);
            if (possibleOrder.Any())
                possibleOrders += 1;
        }
        if (requiredOrders >= possibleOrders)
            return true;
        return false;
    }

    void CheckOrder(List<BeerOrder> beerOrders, Wholesaler wholesaler)
    {
        if (beerOrders.Count == 0)
        {
            Console.WriteLine("order is empty");    
        }
        if (wholesaler == null)
        {
            Console.WriteLine("wholesaler must exist");
        }
        
        var allUnique = beerOrders.Distinct();
        if (allUnique.Count() != beerOrders.Count)
        {
            Console.WriteLine("there can't be any duplicates in the order");
        }

        foreach (var beerOrder in beerOrders)
        {
            var availableBeers = wholesaler.Beers;
            var beerAvaliable = availableBeers.Any(b => b.Name == beerOrder.BeerName);
            if (!beerAvaliable)
            {
                Console.WriteLine("the beer must be sold by the wholesaler");
            }

            var beerInStock = (
                from b in availableBeers 
                where b.Name == beerOrder.BeerName 
                && b.Amount >= beerOrder.BeerAmount
                select 1).Any();

            if (!beerInStock)
            {
                Console.WriteLine("the number of beers ordered cannot be greater than the wholesaler's stock");
            }

        }
        
        
        
        
            
    }
    // Reason an order might not be possible:
    //- order is empty
    //- wholesaler must exist
    //- there can't be any duplicates in the order; 
    //- the number of beers ordered cannot be greater than the wholesaler's stock; 
    //- the beer must be sold by the wholesaler
    
    // Calculate the price of a quote -> 
    List<BeerOrder> AddPriceToBeerOrders(List<BeerOrder> beerOrders, List<Beer> beers)
    {
        foreach (var beerOrder in beerOrders)
        {
            var beerName = beerOrder.BeerName;
            var price = (from Beer in beers where beerName == Beer.Name select Beer.Price).FirstOrDefault();
            //var price = beers.Where(b => b.Name == beerName).FirstOrDefault().Price;
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