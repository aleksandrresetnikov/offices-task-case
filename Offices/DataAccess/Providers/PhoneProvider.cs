using Offices.Models.Entities;

namespace Offices.DataAccess.Providers;

public class PhoneProvider : ProviderBase<Phone>
{
    public PhoneProvider(DellinDictionaryDbContext context) : base(context)
    {
    }
}