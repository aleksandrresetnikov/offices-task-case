using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Offices.Utils;

namespace Offices.Models.Dto.Shared;

public class PaginationRequest
{
    [DefaultValue(0)]
    [FromQuery(Name = "page")]
    public int Page { get; set; }
    
    [DefaultValue(20)]
    [FromQuery(Name = "count")]
    public int Count { get; set; }

    public override string ToString()
    {
        var keyStr = $"{Page}-{Count}";
        return HashUtil.HashSHA256(keyStr);
    }
}