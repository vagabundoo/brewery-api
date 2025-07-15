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
        var wholesalerBeer = wholesaler.Beers 
            .FirstOrDefault(wb => wb.BeerId == beerId);

        if (wholesalerBeer != null)
        {
            wholesalerBeer.Amount = amount;
        }
        else
        {
            wholesaler.Beers.Add(new WholesalerBeer
            {
                WholesalerId = wholesalerId,
                BeerId = beerId,
                Amount = amount,
            });
        }
        await _db.SaveChangesAsync();
        return wholesaler;
        
    }
        
        



        /*var wholesalerBeers = wholesaler.Beers.FirstOrDefault(b => b. == beerId);
        if (wholesalerBeers != null)
        {
            wholesalerBeers.Amount += amount;

            await _db.SaveChangesAsync();
            return wholesaler;
        }

        wholesaler.Beers.Add(beer);
        await _db.SaveChangesAsync();
        return wholesaler;*/
    
}
