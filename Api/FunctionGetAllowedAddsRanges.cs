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
    public class FunctionGetAllowedAddsRanges
    {
        readonly ILogger<FunctionGetAllowedAddsRanges> _logger;

        public FunctionGetAllowedAddsRanges(ILogger<FunctionGetAllowedAddsRanges> logger) => _logger = logger;

        [Function("FunctionGetAllowedAddsRanges")]
        public IActionResult Run(
                [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "FunctionGetAllowedAddsRanges")] HttpRequestData req,
                 [SqlInput("EXEC Bonus.GetAllowedAddsRanges", "AZURESQL_CONNECTION_STRING")] IEnumerable<AllowedAddsRange> result
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
