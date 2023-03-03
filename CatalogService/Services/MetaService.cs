using CatalogService.Models;

namespace CatalogService.Services;

public class MetaService : IMetaService
{
    public async Task<IEnumerable<MetaItem>> FindAll()
    {
        return new List<MetaItem>
        {
            Capacity = 10
        };
    }

    public async Task<MetaItem?> Find(string id)
    {
        return new MetaItem
        {
            Id = default,
            Updated = default,
            Name = null,
            Description = null,
            Release = null,
            Franchise = null,
            FileContainerId = default
        };
    }
}