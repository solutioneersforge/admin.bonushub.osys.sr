using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Enums;
using Shared.Extentions;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace Api
{
    public class FunctionUpdateAllowedAddsRange
    {
        readonly ILogger<FunctionUpdateAllowedAddsRange> _logger;

        public FunctionUpdateAllowedAddsRange(ILogger<FunctionUpdateAllowedAddsRange> logger) => _logger = logger;

        [Function("FunctionUpdateAllowedAddsRange")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "FunctionUpdateAllowedAddsRange")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

#if !DEBUG
            var user = Authentication.StaticWebAppsApiAuth.Parse(req);
            if (!user.IsInRole(AuthRoles.Admin.GetDescription()))
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
#endif

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var parameters = JsonConvert.DeserializeObject<AllowedAddsRange>(requestBody);

            try
            {
                var result = await ExecuteStoredProcedureAsync(parameters);
                if (result)
                {
                    return new OkObjectResult("Success!");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error executing stored procedure: {ex.Message}");
            }

            return new OkObjectResult("Error!");
        }

        async Task<bool> ExecuteStoredProcedureAsync(AllowedAddsRange item)
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURESQL_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }

            using (SqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new("Bonus.UpdateAllowedAddsRange", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = item.Id });
                    command.Parameters.Add(new SqlParameter("@MinimumAdds", SqlDbType.Int) { Value = item.MinimumAdds });
                    command.Parameters.Add(new SqlParameter("@MaximumAdds", SqlDbType.Int) { Value = item.MaximumAdds });
                    command.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar) { Value = item.User });

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
