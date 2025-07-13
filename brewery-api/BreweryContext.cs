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
              modelBuilder.Entity<Beer>()
                     .HasOne<Brewery>()
                     .WithMany()
                     .HasForeignKey(b => b.BreweryId);

              modelBuilder.Entity<Beer>()
                     .HasMany<Wholesaler>()
                     .WithMany(w => w.Beers)
                     .UsingEntity(j => j.ToTable("WholesalerBeers"));
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
       public int Amount { get; set; }
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

       public List<Beer> Beers { get; set; } = new(); 
}

