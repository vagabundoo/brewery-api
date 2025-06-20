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

            // Create
            var brewery = new Brewery { Name = "Leffe" };
            db.Breweries.Add(brewery);
            await db.SaveChangesAsync();

            // Read
            var retrieved = await db.Breweries
                .OrderBy(b => b.BreweryId)
                .FirstOrDefaultAsync();

            if (retrieved != null)
            {
                // Update: add new beer
                retrieved.Beers.Add(new Beer
                {
                    Name = "Leffe Blond",
                    Price = 5.0,
                    Stock = 200,
                    BreweryId = retrieved.BreweryId
                });
                await db.SaveChangesAsync();
                
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
            }

            Console.WriteLine("Done!");
        }
    }
}