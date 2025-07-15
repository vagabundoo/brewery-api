using Microsoft.EntityFrameworkCore;

namespace brewery_api.Services;

public class WholesalerService
{
    private readonly BreweryContext _db;
    public WholesalerService(BreweryContext db) => _db = db;

    public async Task<List<Wholesaler>> GetAllAsync()
    {
        return await _db.Wholesalers.ToListAsync();
    }

    public async Task<Wholesaler?> GetByIdAsync(int id)
    {
        return await _db.Wholesalers.FindAsync(id);
    }

    public async Task<Wholesaler> Create(string name)
    {
        var wholesaler = new Wholesaler() { Name = name };
        _db.Wholesalers.Add(wholesaler);
        await _db.SaveChangesAsync();
        return wholesaler;
    }

    public async Task<Wholesaler?> BuyBeer(int wholesalerId, int beerId, int amount)
    {
        var wholesaler = _db.Wholesalers
            .Include(wholesaler => wholesaler.Beers)
            .FirstOrDefault(w => w.Id == wholesalerId);
        if (wholesaler == null)
        {
            return null;
        }

        var beer = _db.Beers.FirstOrDefault(b => b.Id == beerId);
        if (beer == null)
        {
            return null;
        }

        beer.Amount += amount;

        var wholesalerBeers = wholesaler.Beers;
        wholesalerBeers.Add(beer);
        await _db.SaveChangesAsync();
        return wholesaler;
    }
}
