using CatalogService.Models;

namespace CatalogService.Services;

public interface IFranchiseService
{
    Task<IEnumerable<Franchise>> FindAll();
    Task<Franchise?> Find(Guid id);
    Task<Guid> Create(Franchise franchise);
    Task<Guid> Update(Guid id, Franchise franchise);
    Task<Guid?> Delete(Guid id);
}