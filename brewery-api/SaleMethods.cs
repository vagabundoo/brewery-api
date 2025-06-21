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
    
    // Calculate the price of a quote
    double GetQoutePrice(List<BeerOrder> beerOrders)
    {
        double totalPrice = 0;

        foreach (var beerOrder in beerOrders)
        {
            totalPrice += beerOrder.BeerAmount *  beerOrder.BeerPrice; 
        }
        
        return totalPrice;
    }

}

public struct BeerOrder(string beerName, int beerAmount, double beerPrice)
{
    public string BeerName = beerName;
    public int BeerAmount = beerAmount;
    public double BeerPrice = beerPrice;
}