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
       
// Each brewer has its own list of beers, they are not shared.
// Which means we can set a price and stock, and it will be linked to only one brewer.
public class Beer
{
       public int BeerId { get; set; }
       public string Name { get; set; }
       public double Price { get; set; }
       public int Stock { get; set; }
       
       public int BreweryId { get; set; }
}