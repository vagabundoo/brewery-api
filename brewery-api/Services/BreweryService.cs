using Microsoft.EntityFrameworkCore;

namespace brewery_api.Services;

public class BreweryService
{
    private readonly BreweryContext _db;
    public BreweryService(BreweryContext db) => _db = db;
    
    public async Task<List<Brewery>> GetAllAsync()
    {
        return await _db.Breweries.ToListAsync();
    }

    public async Task<Brewery?> GetByIdAsync(int id)
    {
        return await _db.Breweries.FindAsync(id);
    }
    
    public async Task<Brewery> Create(string name)
    {
        var brewery = new Brewery() { Name = name };
        _db.Breweries.Add(brewery);
        await _db.SaveChangesAsync();
        return brewery;
    }
}