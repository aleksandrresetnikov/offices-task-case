namespace Offices.Models.Settings;

public class OriginSettings
{
    public required string FrontendOrigins { get; set; }

    public string[] GetOriginsArray() => 
        FrontendOrigins.Split(';', StringSplitOptions.RemoveEmptyEntries);
}