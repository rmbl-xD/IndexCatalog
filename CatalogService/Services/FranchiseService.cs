using CatalogService.Data;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services;

public class FranchiseService : IFranchiseService
{
    private readonly ILogger<MetaService> _log;
    private readonly DataContext _context;

    public FranchiseService(ILogger<MetaService> log, DataContext context)
    {
        _log = log;
        _context = context;
    }
    
    public async Task<IEnumerable<Franchise>> FindAll()
    {
        var items = await _context.Franchises.ToListAsync();
        return items;
    }

    public async Task<Franchise?> Find(Guid id)
    {
        var item = await _context.Franchises.FindAsync(id);
        return item;
    }

    public async Task<Guid> Create(Franchise franchise)
    {
        var item = await _context.Franchises.AddAsync(franchise);
        await _context.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<Guid> Update(Guid id, Franchise franchise)
    {
        var item = _context.Franchises.Update(franchise);
        await _context.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<Guid?> Delete(Guid id)
    {
        var franchise = await Find(id);
        
        if (franchise == null) return null;
        
        var item = _context.Franchises.Remove(franchise);
        await _context.SaveChangesAsync();
        return item.Entity?.Id;
    }
}