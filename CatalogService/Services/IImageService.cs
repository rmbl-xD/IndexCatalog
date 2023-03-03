using CatalogService.Models;

namespace CatalogService.Services;

public interface IImageService
{
    Task Upload(FileModel fileModel);
    Task<Stream?> Find(string id);
}