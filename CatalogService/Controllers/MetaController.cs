using System.Text.Json;
using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using StackExchange.Redis;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class MetaController : ControllerBase
{
    private readonly ILogger<MetaController> _logger;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IConnectionMultiplexer _redis;
    private readonly IMetaService _metaService;

    public MetaController(ILogger<MetaController> logger, IOutputCacheStore outputCacheStore,
        IConnectionMultiplexer redis, IMetaService metaService)
    {
        _logger = logger;
        _outputCacheStore = outputCacheStore;
        _redis = redis;
        _metaService = metaService;
    }

    [HttpGet]
    [OutputCache]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MetaItem>>> Get()
    {
        try
        {
            //database
            var metaItems = await _metaService.FindAll();
            if (metaItems.Any()) return Ok(metaItems);

            //nothing found
            return Ok(Enumerable.Empty<MetaItem>());
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MetaItem>> Get(Guid id)
    {
        try
        {
            var meta = await _metaService.Find(id);
            if (meta is null) return NotFound();
            return Ok(meta);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Post([FromBody] MetaItem metaItem)
    {
        try
        {
            var metaItemId = await _metaService.Create(metaItem);
            return Ok(metaItemId);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Put(Guid metaItemId, [FromBody] MetaItem metaItem)
    {
        try
        {
            var id = await _metaService.Update(metaItemId, metaItem);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpDelete("{metaItemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(Guid metaItemId)
    {
        try
        {
            var id = await _metaService.Delete(metaItemId);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}