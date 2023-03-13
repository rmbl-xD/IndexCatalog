using CatalogService.Models;

namespace CatalogService.Services;

public interface IMetaService
{ 
    Task<IEnumerable<MetaItem>> FindAll();
    Task<MetaItem?> Find(Guid id);
    Task<Guid> Create(MetaItem metaItem);
    Task<Guid> Update(MetaItem metaItem);
    Task<Guid?> Delete(Guid id);
}