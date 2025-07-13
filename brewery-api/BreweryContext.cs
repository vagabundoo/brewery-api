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
              // Configure one-to-many: Brewery -> Beers
              modelBuilder.Entity<Beer>()
                     .HasOne(b => b.Brewery)
                     .WithMany(br => br.Beers)
                     .HasForeignKey(b => b.BreweryId)
                     .OnDelete(DeleteBehavior.SetNull);

              // Configure many-to-many: Beer <-> Wholesaler
              modelBuilder.Entity<Beer>()
                     .HasMany(b => b.Wholesalers)
                     .WithMany(w => w.Beers)
                     .UsingEntity(j => j.ToTable("WholesalerBeers"));

              //modelBuilder.Entity<Wholesaler>();
       }
}

// Each brewer has its own list of beers, they are not shared.
// Which means we can set a price, and it will be linked to only one brewer.
// No amount is needed for breweries, but it is needed for wholesalers.
public class Beer
{
       [Key]
       public int Id { get; set; } 

       public required string Name { get; set; }
       public double Price { get; set; }
       public int Amount { get; set; }

       public int BreweryId { get; set; } // Foreign key
       public Brewery? Brewery { get; set; }

       public List<Wholesaler> Wholesalers { get; set; } = new(); // many-to-many
}

public class Brewery
{
       public int Id { get; set; }
       public string Name { get; set; }

       public List<Beer> Beers { get; set; } = new(); // one-to-many
}

public class Wholesaler
{
       public int Id { get; set; } 
       public string Name { get; set; }

       public List<Beer> Beers { get; set; } = new(); // many-to-many
}

