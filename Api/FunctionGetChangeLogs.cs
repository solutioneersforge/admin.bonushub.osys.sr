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
    public class FunctionGetChangeLogs
    {
        readonly ILogger<FunctionGetChangeLogs> _logger;

        public FunctionGetChangeLogs(ILogger<FunctionGetChangeLogs> logger) => _logger = logger;

        [Function("FunctionGetChangeLogs")]
        public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FunctionGetChangeLogs/{bd}/{ed}")] HttpRequestData req,
                [SqlInput(commandText: "Bonus.GetChangeLogs",
                    commandType: System.Data.CommandType.StoredProcedure,
                    parameters: "@Bd={bd},@Ed={ed}",
                    connectionStringSetting: "AZURESQL_CONNECTION_STRING")] IEnumerable<Changelog> result
                )
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

#if !DEBUG
            var user = Authentication.StaticWebAppsApiAuth.Parse(req);
            if (!user.IsInRole(AuthRoles.Admin.GetDescription()))
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
#endif

            return new OkObjectResult(result);
        }
    }
}
