using Data.Models;
using System.Collections.Generic;

namespace Data.AzureFunctionResponse;

public class AfrSetting
{
    public IEnumerable<Setting> Value { get; set; }
    public int StatusCode { get; set; }
}
