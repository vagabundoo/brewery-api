using brewery_api;
using brewery_api.Services;

namespace brewery_unit_tests;

public class ClientQuoteTests
{

    [Test]
    public void HappyPath()
    {
        var beerOrders = new List<BeerOrder>
        {
            new BeerOrder(
                "Leffe Blond",
                30
            ), 
        };
        var beers = new List<Beer>
            {
                new Beer()
                {
                    Name = "Leffe Blond",
                    BreweryId = 1,
                    Price = 5,
                },
            };
        var service = new ClientService();
        var wholesaler = new Wholesaler
        {
            Id = 1,
            Name = "BeersRUs",
            Beers = new List<Beer>
            {
                new Beer()
                {
                    Name = "Leffe Blond",
                    BreweryId = 1,
                    Amount = 50,
                }
            },
        };
        var quote = service.GetQuote(beerOrders, wholesaler, beers);
        
        Assert.That(quote.TextSummary, Is.EqualTo("Beer: Leffe Blond, Amount: 30, Price = 5"));
    }
    
    [Test]
    public void ValidOrder()
    {
        var beerOrders = new List<BeerOrder>
        {
            new BeerOrder(
                "Leffe Blond",
                30
            ), 
        };
        var beers = new List<Beer>
        {
            new Beer()
            {
                Name = "Leffe Blond",
                BreweryId = 1,
                Price = 5,
            },
        };
        var service = new ClientService();
        var wholesaler = new Wholesaler
        {
            Id = 1,
            Name = "BeersRUs",
            Beers = new List<Beer>
            {
                new Beer()
                {
                    Name = "Leffe Blond",
                    BreweryId = 1,
                    Amount = 50,
                }
            },
        };
        var quote = service.GetQuote(beerOrders, wholesaler, beers);
        
        Assert.That(quote.TextSummary, Is.EqualTo("Beer: Leffe Blond, Amount: 30, Price = 5"));
    }
    
    [Test]
    public void EmptyOrder()
    {
        var beerOrders = new List<BeerOrder>
        {
            
        };
        var beers = new List<Beer>
        {};
        var service = new ClientService();
        var wholesaler = new Wholesaler { };
        var quote = service.GetQuote(beerOrders, wholesaler, beers);
        
        Assert.That(quote.ReasonOrderInvalid, Is.EqualTo("Order is empty"));
    }
    
    [Test]
    public void WholesalerDoesNotExist()
    {
        var beerOrders = new List<BeerOrder>
        {
            new BeerOrder(
                "Leffe Blond",
                30
            ), 
        };
        var beers = new List<Beer>
        {
            new Beer()
            {
                Name = "Leffe Blond",
                BreweryId = 1,
                Price = 5,
            },
        };
        var service = new ClientService();
        var quote = service.GetQuote(beerOrders, null, beers);
        
        Assert.That(quote.ReasonOrderInvalid, Is.EqualTo("Wholesaler must exist"));
    }
    
    [Test]
    public void DuplicateOrder()
    {
        var beerOrders = new List<BeerOrder>
        {
            new BeerOrder(
                "Leffe Blond",
                30
            ), 
            new BeerOrder(
                "Leffe Blond",
                30
            ), 
            
        };
        var beers = new List<Beer>
            {};
        var service = new ClientService();
        var wholesaler = new Wholesaler { };
        var quote = service.GetQuote(beerOrders, wholesaler, beers);
        
        Assert.That(quote.ReasonOrderInvalid, Is.EqualTo("There can't be any duplicates in the order"));
    }
    
    [Test]
    public void BeerNotSoldByWholesaler()
    {
        var beerOrders = new List<BeerOrder>
        {
            new BeerOrder(
                "Leffe Blond",
                30
            ), 
        };
        var beers = new List<Beer>
            {};
        var service = new ClientService();
        var wholesaler = new Wholesaler { };
        var quote = service.GetQuote(beerOrders, wholesaler, beers);
        
        Assert.That(quote.ReasonOrderInvalid, Is.EqualTo("The beer must be sold by the wholesaler"));
    }
    
    [Test]
    public void OrderAmountGreaterThanWholesalerStock()
    {
        var beerOrders = new List<BeerOrder>
        {
            new BeerOrder(
                "Leffe Blond",
                11
            ), 
        };
        var beers = new List<Beer>
        {
            new Beer()
            {
                Name = "Leffe Blond",
                BreweryId = 1,
                Price = 5,
            },
        };
        var service = new ClientService();
        var wholesaler = new Wholesaler
        {
            Id = 1,
            Name = "BeersRUs",
            Beers = new List<Beer>
            {
                new Beer()
                {
                    Name = "Leffe Blond",
                    BreweryId = 1,
                    Amount = 10,
                }
            },
        };
        var quote = service.GetQuote(beerOrders, wholesaler, beers);
        
        Assert.That(quote.ReasonOrderInvalid, Is.EqualTo("The number of beers ordered cannot be greater than the wholesaler's stock"));
    }
}