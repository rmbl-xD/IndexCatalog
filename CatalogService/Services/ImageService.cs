using Azure.Storage.Blobs;
using CatalogService.Models;

namespace CatalogService.Services;

public class ImageService : IImageService
{
    private readonly ILogger<ImageService> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public ImageService(ILogger<ImageService> logger, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    public async Task Upload(Guid metaItemId, FileModel fileModel)
    {
        try
        {
            var container = _blobServiceClient.GetBlobContainerClient($"images/{metaItemId}");
            var blobInstance = container.GetBlobClient(fileModel.ImageFile.FileName);
            await blobInstance.UploadAsync(fileModel.ImageFile.OpenReadStream());
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
        }
    }

    public async Task<Stream?> Find(Guid metaItemId, Guid id)
    {
        try
        {
            var container = _blobServiceClient.GetBlobContainerClient($"images/{metaItemId}");
            var blobInstance = container.GetBlobClient(id.ToString());
            var downloadContent = await blobInstance.DownloadAsync();
            return downloadContent.Value.Content;
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
        }

        return null;
    }
}