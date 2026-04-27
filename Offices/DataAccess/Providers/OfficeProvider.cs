using Offices.Models.Entities;

namespace Offices.DataAccess.Providers;

public class OfficeProvider : ProviderBase<Office>
{
    public OfficeProvider(DellinDictionaryDbContext context) : base(context)
    {
    }
}