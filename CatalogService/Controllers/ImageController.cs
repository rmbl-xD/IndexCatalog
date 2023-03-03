using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IConnectionMultiplexer _redis;
    private readonly IImageService _imageService;

    public ImageController(ILogger<ImageController> logger, IOutputCacheStore outputCacheStore, IConnectionMultiplexer redis, IImageService imageService)
    {
        _logger = logger;
        _outputCacheStore = outputCacheStore;
        _redis = redis;
        _imageService = imageService;
    }
    
    [HttpGet("{id}")]
    [OutputCache(Duration = 60*10, VaryByQueryKeys = new [] { "id" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return BadRequest();
        
        try
        {
            var imageFileStream = await _imageService.Find(id);

            var fileType = "jpeg";
            if (id.Contains("png"))
            {
                fileType = "png";
            }

            return File(imageFileStream, $"image/{fileType}");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Upload([FromForm] FileModel file)
    {
        try
        {
            await _imageService.Upload(file);
            return Ok("success");
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}