using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Offices.Models.Entities;

public class Phone : TypeBase
{
    [Required]
    [ForeignKey("Office")]
    public int OfficeId { get; set; }
    public Office Office { get; set; }

    [Required][Phone][MaxLength(128)]
    public string PhoneNumber { get; set; }

    public string? Additional { get; set; }

}