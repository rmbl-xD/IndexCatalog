namespace CatalogService.Models;

public class FileModel
{
    public FileModel(IFormFile imageFile)
    {
        ImageFile = imageFile;
    }

    public IFormFile ImageFile { get; set; }
}