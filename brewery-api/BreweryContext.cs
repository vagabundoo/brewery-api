namespace brewery_api;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

public class BreweryContext : DbContext
{
       public DbSet<Brewery> Breweries { get; set; }
       public DbSet<Beer>  Beers { get; set; }
       public DbSet<Wholesaler> Wholesalers { get; set; }
       
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
       public int Id { get; set; }
       public string Name { get; set; }

       public List<Beer> Beers { get; set; } = new();

}

public class  Wholesaler 
{
       public int Id { get; set; }
       
       public string Name { get; set; }
       
       public List<Beer> Beers { get; } = new();

       void buyBeers()
       {
       }
}
       
// Each brewer has its own list of beers, they are not shared.
// Which means we can set a price, and it will be linked to only one brewer.
// No amount is needed for breweries, but it is needed for wholesalers.
public class Beer
{
       public int Id { get; set; }
       public int BreweryId { get; set; }
       public string? Name { get; set; }
       public double Price { get; set; }
       public int? Amount { get; set; }
}