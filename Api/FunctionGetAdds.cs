using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Sql;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using Shared.Extentions;
using System.Collections.Generic;

namespace Api
{
    public class FunctionGetAdds
    {
        readonly ILogger<FunctionGetAdds> _logger;

        public FunctionGetAdds(ILogger<FunctionGetAdds> logger) => _logger = logger;

        [Function("FunctionGetAdds")]
        public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FunctionGetAdds/{bd}/{ed}")] HttpRequestData req,
                [SqlInput(commandText: "Bonus.GetAdds",
                    commandType: System.Data.CommandType.StoredProcedure,
                    parameters: "@Bd={bd},@Ed={ed}",
                    connectionStringSetting: "AZURESQL_CONNECTION_STRING")] IEnumerable<Add> result
                )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var user = Authentication.StaticWebAppsApiAuth.Parse(req);
            if (!user.IsInRole(AuthRoles.Admin.GetDescription()))
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

            return new OkObjectResult(result);
        }
    }
}
