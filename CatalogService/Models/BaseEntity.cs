using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class BaseEntity
{ 
    [Key]
    public Guid Id { get; set; }
 
    public DateTimeOffset? Updated { get; set; }
    
    [Required]
    public DateTimeOffset Created { get; set; }
}