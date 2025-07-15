using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

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
              DbPath = System.IO.Path.Combine("../brewery.db");
       }
       
       protected override void OnConfiguring(DbContextOptionsBuilder options)
              => options
                     .UseSqlite($"Data Source={DbPath}")
                     .LogTo(message => Debug.WriteLine(message));

       protected override void OnModelCreating(ModelBuilder modelBuilder)
       {
              modelBuilder.Entity<WholesalerBeer>()
                     .HasKey(wb => new { wb.WholesalerId, wb.BeerId });

              modelBuilder.Entity<WholesalerBeer>()
                     .HasOne(wb => wb.Wholesaler)
                     .WithMany(w => w.Beers)
                     .HasForeignKey(wb => wb.WholesalerId);

              modelBuilder.Entity<Beer>()
                     .HasOne<Brewery>();

       }
}

// Each brewer has its own list of beers, they are not shared.
// Which means we can set a price, and it will be linked to only one brewer.
// No amount is needed for breweries, but it is needed for wholesalers.
public class Beer
{
       [Key]
       public int Id { get; set; } 

       [MaxLength(30)]
       public required string Name { get; set; }
       public double Price { get; set; }
       public int BreweryId { get; set; } 
       
}

public class Brewery
{
       [Key]
       public int Id { get; set; }
       [MaxLength(30)]
       public required string Name { get; set; }
}

public class Wholesaler
{
       public int Id { get; set; } 
       public string Name { get; set; }

       public ICollection<WholesalerBeer> Beers { get; set; } = new List<WholesalerBeer>();
}

public class WholesalerBeer
{
       public int WholesalerId { get; set; }
       public Wholesaler Wholesaler { get; set; }
       
       public int BeerId { get; set; }
       public int Amount { get; set; }
       
}

