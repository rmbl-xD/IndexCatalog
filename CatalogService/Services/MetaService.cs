using CatalogService.Data;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Services;

public class MetaService : IMetaService
{
    private readonly ILogger<MetaService> _log;
    private readonly DataContext _context;

    public MetaService(ILogger<MetaService> log, DataContext context)
    {
        _log = log;
        _context = context;
    }
    
    public async Task<IEnumerable<MetaItem>> FindAll()
    {
        var items = await _context.MetaItems.ToListAsync();
        return items;
    }

    public async Task<MetaItem?> Find(Guid id)
    {
        var item = await _context.MetaItems.FindAsync(id);
        return item;
    }

    public async Task<Guid> Create(MetaItem metaItem)
    {
        var item = await _context.MetaItems.AddAsync(metaItem);
        await _context.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<Guid> Update(MetaItem metaItem)
    {
        var item = _context.MetaItems.Update(metaItem);
        await _context.SaveChangesAsync();
        return item.Entity.Id;
    }

    public async Task<Guid?> Delete(Guid id)
    {
        var metaItem = await Find(id);
        var item = _context.MetaItems.Remove(metaItem);
        await _context.SaveChangesAsync();
        return item.Entity?.Id;
    }
}