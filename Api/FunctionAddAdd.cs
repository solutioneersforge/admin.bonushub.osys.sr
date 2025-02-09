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

        async Task<bool> ExecuteStoredProcedureAsync(Add item)
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

                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = item.Id });
                    command.Parameters.Add(new SqlParameter("@AddPlacementZone", SqlDbType.NVarChar) { Value = item.AddPlacementZone });
                    command.Parameters.Add(new SqlParameter("@LabelText", SqlDbType.NVarChar) { Value = item.LabelText });
                    command.Parameters.Add(new SqlParameter("@BonusAmountText", SqlDbType.NVarChar) { Value = item.BonusAmountText });
                    command.Parameters.Add(new SqlParameter("@ImageBgColor", SqlDbType.NVarChar) { Value = item.ImageBgColor });
                    command.Parameters.Add(new SqlParameter("@FontFamily", SqlDbType.NVarChar) { Value = item.FontFamily });
                    command.Parameters.Add(new SqlParameter("@FontSize", SqlDbType.NVarChar) { Value = item.FontSize });
                    command.Parameters.Add(new SqlParameter("@ImageFileName", SqlDbType.NVarChar) { Value = item.ImageFileName });
                    command.Parameters.Add(new SqlParameter("@ImageBlobName", SqlDbType.NVarChar) { Value = item.ImageBlobName });
                    command.Parameters.Add(new SqlParameter("@RedirectUrl", SqlDbType.NVarChar) { Value = item.RedirectUrl });
                    command.Parameters.Add(new SqlParameter("@ActiveFrom", SqlDbType.Date) { Value = item.ActiveFrom });
                    command.Parameters.Add(new SqlParameter("@ActiveUntil", SqlDbType.Date) { Value = item.ActiveUntil });
                    command.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit) { Value = item.IsActive });
                    command.Parameters.Add(new SqlParameter("@IsPublished", SqlDbType.Bit) { Value = item.IsPublished });
                    command.Parameters.Add(new SqlParameter("@DisplayOrder", SqlDbType.Int) { Value = item.DisplayOrder });
                    command.Parameters.Add(new SqlParameter("@User", SqlDbType.NVarChar) { Value = item.User });

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
