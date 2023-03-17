using CatalogService.Models;

namespace CatalogService.Services;

public interface IImageService
{
    Task Upload(Guid metaItemId, FileModel fileModel);
    Task<Stream?> Find(Guid metaItemId, Guid id);
}