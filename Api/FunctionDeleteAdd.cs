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
    public class FunctionDeleteAdd
    {
        readonly ILogger<FunctionDeleteAdd> _logger;

        public FunctionDeleteAdd(ILogger<FunctionDeleteAdd> logger) => _logger = logger;

        [Function("FunctionDeleteAdd")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "FunctionDeleteAdd")] HttpRequestData req)
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
            var parameters = JsonConvert.DeserializeObject<Add>(requestBody);

            try
            {
                var result = await ExecuteStoredProcedureAsync(parameters);
                if (result)
                {
                    //return new StatusCodeResult(StatusCodes.Status200OK);
                    return new OkObjectResult("Success!");
                }
                //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error executing stored procedure: {ex.Message}");
                //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return new OkObjectResult("Error!");
        }

        async Task<bool> ExecuteStoredProcedureAsync(Add add)
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURESQL_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }

            using (SqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new("Bonus.DeleteAdd", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = add.Id });
                    command.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar) { Value = add.User });

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
