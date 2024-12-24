using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Api
{
    public class FunctionGetAdds
    {
        private readonly ILogger<FunctionGetAdds> _logger;
        //private const string StoredProcedure = "GetAdds";

        public FunctionGetAdds(ILogger<FunctionGetAdds> logger)
        {
            _logger = logger;
        }

        [Function("FunctionGetAdds")]
        public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FunctionGetAdds/{state}")] HttpRequestData req,
                [SqlInput(commandText: "GetAdds",
                    commandType: System.Data.CommandType.StoredProcedure,
                     parameters: "@State={state}",
                    connectionStringSetting: "AZURESQL_CONNECTION_STRING")] IEnumerable<Add> result
                )
        {
            return new OkObjectResult(result);
        }
    }
}
