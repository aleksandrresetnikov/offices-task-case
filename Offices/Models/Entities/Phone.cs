namespace Offices.Models.Entities;

public class Phone : TypeBase
{
    // Связанный с номером телефона офис
    public int OfficeId { get; set; }
    public Office Office { get; set; }

    public string PhoneNumber { get; set; }

    public string? Additional { get; set; }

}