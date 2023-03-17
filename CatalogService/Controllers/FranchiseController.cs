using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class FranchiseController : ControllerBase
{
    private readonly ILogger<FranchiseController> _logger;
    private readonly IFranchiseService _franchiseService;

    public FranchiseController(ILogger<FranchiseController> logger, IFranchiseService franchiseService)
    {
        _logger = logger;
        _franchiseService = franchiseService;
    }
    
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Franchise>>> Get()
    {
        try
        {
            var franchises = await _franchiseService.FindAll();
            
            switch (franchises.Any())
            {
                case true:
                    return Ok(franchises);
                default:
                    return Ok(Enumerable.Empty<Franchise>());
            }
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Franchise>> Get(Guid id)
    {
        try
        {
            var franchise = await _franchiseService.Find(id);
            if (franchise is null) return NotFound();
            return Ok(franchise);
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
    public async Task<ActionResult<Guid>> Post([FromBody] Franchise franchise)
    {
        try
        {
            var id = await _franchiseService.Create(franchise);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPut("{franchiseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Put(Guid franchiseId, [FromBody] Franchise franchise)
    {
        try
        {
            var id = await _franchiseService.Update(franchiseId, franchise);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpDelete("{franchiseId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid>> Delete(Guid franchiseId)
    {
        try
        {
            var id = await _franchiseService.Delete(franchiseId);
            return Ok(id);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
}