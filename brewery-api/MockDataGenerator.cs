using System;
using System.Linq;
using System.Threading.Tasks;
using brewery_api;
using Microsoft.EntityFrameworkCore;

namespace brewery_api
{
    public class MockDataGenerator(BreweryContext db)
    {
        public async Task GenerateAsync()
        {
            // Reset database
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Create brewery
            var brewery = new Brewery { Name = "Leffe" };
            db.Breweries.Add(brewery);
            await db.SaveChangesAsync();
            
            // Read
            var retrieved = await db.Breweries
                .OrderBy(b => b.Id)
                .FirstOrDefaultAsync();

            if (retrieved != null)
            {
                // Update: add new beer
                retrieved.Beers.Add(new Beer
                {
                    Name = "Leffe Blond",
                    Price = 8.0,
                    BreweryId = retrieved.Id
                });
                retrieved.Beers.Add(new Beer
                {
                    Name = "Leffe",
                    Price = 5.0,
                    BreweryId = retrieved.Id
                });
                await db.SaveChangesAsync();
                
                /*
                // Update: change price of exisiting beer
                double newPrice = 10.0;
                var specificBeer = await db.Beers
                    .Where(b => b.BreweryId == retrieved.BreweryId)
                    .FirstOrDefaultAsync();
                
                if  (specificBeer != null)
                    specificBeer.Price =  newPrice;
                await db.SaveChangesAsync();

                // Delete
                db.Breweries.Remove(retrieved);
                await db.SaveChangesAsync();
                */
                /*
                // Create wholesaler
                var wholesaler = new Wholesaler { Name = "Belgian Beers" };
                db.Wholesalers.Add(wholesaler);
                await db.SaveChangesAsync();
                
                // Add beer to wholesaler
                wholesaler.Beers.Add(new Beer
                {
                    Id = retrieved.Beers.First().Id,
                    BreweryId = retrieved.Id,
                    Name = retrieved.Beers.First().Name,
                    Price = retrieved.Beers.First().Price,
                    Amount = 0,
                });
                await db.SaveChangesAsync();
                
                // Buy beer from brewery
                retrieved.Beers.First().Amount = 10;
                await db.SaveChangesAsync();
                
                // Client requests sale of specific beer to specific wholesaler, 10 units
                string beerName = "Leffe Blond";
                int requestedAmount = 10;
                var retrievedBeer =  await db.Wholesalers
                    .Where(w => w.Beers.Any(b => b.Name == beerName))
                    .FirstOrDefaultAsync();
                
                if (retrievedBeer != null)
                {
                    if (requestedAmount > retrievedBeer.Beers.First().Amount)
                    {
                        Console.WriteLine("Sale not possible.");
                    }
                    else
                    {
                        Console.WriteLine("Sale possible.");
                        
                        double salePrice = retrievedBeer.Beers.First().Price * requestedAmount;
                        Console.WriteLine($"Sale price: {salePrice}");
                        
                        // Update wholesaler inventory
                        retrievedBeer.Beers.First().Amount -= requestedAmount;
                        await db.SaveChangesAsync();
                    }
                    
                }*/
                
                
                
            }
            
           
            

            Console.WriteLine("Done!");
        }
    }
}