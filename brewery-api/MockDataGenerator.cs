using System;
using System.Linq;
using System.Threading.Tasks;
using brewery_api;
using Microsoft.EntityFrameworkCore;

namespace brewery_api.Utilities
{
    public class MockDataGenerator
    {
        private readonly BreweryContext _db;

        public MockDataGenerator(BreweryContext db)
        {
            _db = db;
        }

        public async Task GenerateAsync()
        {
            // Reset database
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            // Create
            var brewery = new Brewery { Name = "Leffe" };
            _db.Breweries.Add(brewery);
            await _db.SaveChangesAsync();

            // Read
            var retrieved = await _db.Breweries
                .OrderBy(b => b.BreweryId)
                .FirstOrDefaultAsync();

            if (retrieved != null)
            {
                // Update
                retrieved.Beers.Add(new Beer
                {
                    Name = "Leffe Blond",
                    Price = 5.0,
                    Stock = 200,
                    BreweryId = retrieved.BreweryId
                });
                await _db.SaveChangesAsync();

                // Delete
                _db.Breweries.Remove(retrieved);
                await _db.SaveChangesAsync();
            }

            Console.WriteLine("Done!");
        }
    }
}