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
    public class FunctionAddAdd
    {
        readonly ILogger<FunctionAddAdd> _logger;

        public FunctionAddAdd(ILogger<FunctionAddAdd> logger) => _logger = logger;

        [Function("FunctionAddAdd")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "FunctionAddAdd")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var user = Authentication.StaticWebAppsApiAuth.Parse(req);
            if (!user.IsInRole(AuthRoles.Admin.GetDescription()))
            {
                return new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }

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

                using (SqlCommand command = new("Bonus.AddAdd", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = add.Id });
                    command.Parameters.Add(new SqlParameter("@AddPlacementZone", SqlDbType.NVarChar) { Value = add.AddPlacementZone });
                    command.Parameters.Add(new SqlParameter("@LabelText", SqlDbType.NVarChar) { Value = add.LabelText });
                    command.Parameters.Add(new SqlParameter("@ImageUrl", SqlDbType.NVarChar) { Value = add.ImageUrl });
                    command.Parameters.Add(new SqlParameter("@RedirectUrl", SqlDbType.NVarChar) { Value = add.RedirectUrl });
                    command.Parameters.Add(new SqlParameter("@ActiveFrom", SqlDbType.Date) { Value = add.ActiveFrom });
                    command.Parameters.Add(new SqlParameter("@ActiveUntil", SqlDbType.Date) { Value = add.ActiveUntil });
                    command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = add.IsActive });
                    command.Parameters.Add(new SqlParameter("@IsPublished", SqlDbType.Bit) { Value = add.IsPublished });
                    command.Parameters.Add(new SqlParameter("@DisplayOrder", SqlDbType.Int) { Value = add.DisplayOrder });
                    command.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar) { Value = add.User });


                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
