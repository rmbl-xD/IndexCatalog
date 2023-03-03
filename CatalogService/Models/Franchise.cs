using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class Franchise : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
}