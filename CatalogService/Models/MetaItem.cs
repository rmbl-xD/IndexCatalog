using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class MetaItem : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string? Name { get; set; }
    
    [Required]
    [StringLength(1023)]
    public string? Description { get; set; }
    
    public DateTime? Release { get; set; }
    public Franchise? Franchise { get; set; }

    public Guid? FileContainerId { get; set; }
}