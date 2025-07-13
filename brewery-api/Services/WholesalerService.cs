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
}