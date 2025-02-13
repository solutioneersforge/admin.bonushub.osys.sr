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
    public class FunctionUpdateSettingsByName
    {
        readonly ILogger<FunctionUpdateSettingsByName> _logger;

        public FunctionUpdateSettingsByName(ILogger<FunctionUpdateSettingsByName> logger) => _logger = logger;

        [Function("FunctionUpdateSettingsByName")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "FunctionUpdateSettingsByName")] HttpRequestData req)
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
            var parameters = JsonConvert.DeserializeObject<Setting>(requestBody);

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

        async Task<bool> ExecuteStoredProcedureAsync(Setting item)
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURESQL_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }

            using (SqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new("Bonus.UpdateSettingsByName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@SettingName", SqlDbType.NVarChar) { Value = item.SettingName });
                    command.Parameters.Add(new SqlParameter("@SettingValue", SqlDbType.NVarChar) { Value = item.SettingValue });

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
