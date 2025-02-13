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
    public class FunctionGetSettingsByName
    {
        readonly ILogger<FunctionGetSettingsByName> _logger;

        public FunctionGetSettingsByName(ILogger<FunctionGetSettingsByName> logger) => _logger = logger;

        [Function("FunctionGetSettingsByName")]
        public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FunctionGetSettingsByName/{value}")] HttpRequestData req,
                [SqlInput(commandText: "Bonus.GetSettingsByName",
                    commandType: System.Data.CommandType.StoredProcedure,
                    parameters: "@SettingName={value}",
                    connectionStringSetting: "AZURESQL_CONNECTION_STRING")] IEnumerable<Setting> result
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
