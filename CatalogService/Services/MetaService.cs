using System.Text.Json;
using CatalogService.Caching;
using CatalogService.Data;
using CatalogService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CatalogService.Services;

public class MetaService : IMetaService
{
    private readonly ILogger<MetaService> _log;
    private readonly ICacheService _cache;
    private readonly DataContext _context;

    public MetaService(ILogger<MetaService> log, ICacheService cache, DataContext context)
    {
        _log = log;
        _cache = cache;
        _context = context;
    }
    
    public async Task<IEnumerable<MetaItem>> FindAll()
    {
        var metaItems = await _cache.GetCacheValueAsync<IEnumerable<MetaItem>>("AllMetaItems");
        if (metaItems != null) { return metaItems; }
        
        var items = await _context.MetaItems.ToListAsync();
        await _cache.SetCacheValueAsync<IEnumerable<MetaItem>>("AllMetaItems", items);
        return items;
    }

    public async Task<MetaItem?> Find(Guid id)
    {
        var metaItem = await _cache.GetCacheValueAsync<MetaItem>(id.ToString());
        if (metaItem != null) { return metaItem; }
        
        var item = await _context.MetaItems.FindAsync(id);
        if (item != null)
        {
            await _cache.SetCacheValueAsync(id.ToString(), item);
        }
        return item;
    }

    public async Task<Guid> Create(MetaItem metaItem)
    {
        var item = await _context.MetaItems.AddAsync(metaItem);
        await _context.SaveChangesAsync();

        _cache.DeleteCacheValue("AllMetaItems");
        
        return item.Entity.Id;
    }

    public async Task<Guid> Update(Guid id, MetaItem metaItem)
    {
        var item = _context.MetaItems.Update(metaItem);
        await _context.SaveChangesAsync();
        
        _cache.DeleteCacheValue("AllMetaItems");
        _cache.DeleteCacheValue(id.ToString());
        
        return item.Entity.Id;
    }

    public async Task<Guid?> Delete(Guid id)
    {
        var metaItem = await Find(id);
        
        if (metaItem == null) return null;
        
        var item = _context.MetaItems.Remove(metaItem);
        await _context.SaveChangesAsync();
        
        _cache.DeleteCacheValue("AllMetaItems");
        _cache.DeleteCacheValue(id.ToString());
        
        return item.Entity?.Id;
    }
}