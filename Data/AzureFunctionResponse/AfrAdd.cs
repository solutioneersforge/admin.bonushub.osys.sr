using Data.Models;
using System.Collections.Generic;

namespace Data.AzureFunctionResponse;

public class AfrAdd
{
    public IEnumerable<Add> Value { get; set; }
    public int StatusCode { get; set; }
}
