namespace brewery_api;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class BreweryContext : DbContext
{
       public DbSet<Brewery> Breweries { get; set; }
       public DbSet<Beer>  Beers { get; set; }
       
       public string DbPath { get; }

       public BreweryContext()
       {
              var folder = Environment.SpecialFolder.LocalApplicationData;
              var path = Environment.GetFolderPath(folder);
              DbPath = System.IO.Path.Combine(path, "brewery.db");
       }
       
       protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options.UseSqlite($"Data Source={DbPath}");

}

public class Brewery
{
       public int BreweryId { get; set; }
       public string Name { get; set; }

       public List<Beer> Beers { get; } = new();

}

public class  Wholesaler : Brewery
{
       public int WholesalerId { get; set; }

       void buyBeers()
       {
       }
}


public class Client
{
       public int ClientId { get; set; }
       public List<BeerOrder> BeerOrders { get; } = new();
       
       void getSalesQuote(int orderAmount, string beerName, string wholesalerName)
       {
              // If unsucessful, returns false
              // If succesful, returns true and, a price, and summary of the quote
       }

       void createOrder(List<BeerOrder> orders)
       {
              
       }

       public struct BeerOrder
       {
              private int amount;
              private string beerName;
              private string wholesalerName;
       }
}
       
// Each brewer has its own list of beers, they are not shared.
// Which means we can set a price, and it will be linked to only one brewer.
// No amount is needed for breweries, but it is needed for wholesalers.
public class Beer
{
       public int BeerId { get; set; }
       public int BreweryId { get; set; }
       public string? Name { get; set; }
       public double Price { get; set; }
       public int? Amount { get; set; }
}