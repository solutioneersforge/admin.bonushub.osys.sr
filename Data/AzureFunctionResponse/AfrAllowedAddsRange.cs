using Data.Models;
using System.Collections.Generic;

namespace Data.AzureFunctionResponse;

public class AfrAllowedAddsRange
{
    public IEnumerable<AllowedAddsRange> Value { get; set; }
    public int StatusCode { get; set; }
}
