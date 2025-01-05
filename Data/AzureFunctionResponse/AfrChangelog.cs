using Data.Models;
using System.Collections.Generic;

namespace Data.AzureFunctionResponse;

public class AfrChangelog
{
    public IEnumerable<Changelog> Value { get; set; }
    public int StatusCode { get; set; }
}
