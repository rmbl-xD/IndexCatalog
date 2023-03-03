using CatalogService.Models;

namespace CatalogService.Services;

public interface IFranchiseService
{
    Task<IEnumerable<Franchise>> FindAll();
    Task<Franchise?> Find(string id);
}