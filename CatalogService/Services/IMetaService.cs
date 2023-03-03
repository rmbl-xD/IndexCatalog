using CatalogService.Models;

namespace CatalogService.Services;

public interface IMetaService
{ 
    Task<IEnumerable<MetaItem>> FindAll();
    Task<MetaItem?> Find(string id);
}