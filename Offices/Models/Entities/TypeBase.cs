using System.ComponentModel.DataAnnotations;

namespace Offices.Models.Entities;

public abstract class TypeBase
{
    [Key]
    public int Id { get; set; }
    
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
}