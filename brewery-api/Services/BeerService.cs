using Microsoft.EntityFrameworkCore;

namespace brewery_api.Services;

public class BeerService
{
    private readonly BreweryContext _db;
    public BeerService(BreweryContext db) => _db = db;
    public async Task<List<Beer>> GetAllAsync()
    {
        return await _db.Beers.ToListAsync();
    }

    public async Task<Beer?> GetByIdAsync(int id)
    {
        return await _db.Beers.FindAsync(id);
    }
    
    public async Task<Beer?> UpdatePriceAsync(int id, double price)
    {
        var beer = await _db.Beers.FirstOrDefaultAsync(b => b.Id == id);
        if (beer == null) return null;

        beer.Price = price;
        await _db.SaveChangesAsync();
        return beer;
    }

    public async Task<Beer> CreateBeer(Beer beer)
    {
        await _db.Beers.AddAsync(beer);
        await _db.SaveChangesAsync();
        return beer;
    }
}