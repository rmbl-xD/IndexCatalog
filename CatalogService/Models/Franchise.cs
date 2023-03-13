using System.ComponentModel.DataAnnotations;

namespace CatalogService.Models;

public class Franchise : BaseEntity
{
    public Franchise(string name)
    {
        Name = name;
    }

    [Required]
    [StringLength(255)]
    public string Name { get; set; }
}