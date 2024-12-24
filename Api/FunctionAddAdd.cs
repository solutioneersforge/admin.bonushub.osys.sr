using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Azure.Functions.Worker.Http;
using Azure;
using Data.AzureFunctionResponse;
using Data.Models;

namespace Api
{
    public class FunctionAddAdd
    {
        private readonly ILogger<FunctionAddAdd> _logger;

        public FunctionAddAdd(ILogger<FunctionAddAdd> logger)
        {
            _logger = logger;
        }

        [Function("FunctionAddAdd")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "FunctionAddAdd")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var parameters = JsonConvert.DeserializeObject<Add>(requestBody);
          
            try
            {
                var result = await ExecuteStoredProcedureAsync(parameters);
                //return new OkObjectResult(new { message = "Success", result });
                if (result)
                {
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error executing stored procedure: {ex.Message}");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        async Task<bool> ExecuteStoredProcedureAsync(Add add)
        {
            // Retrieve the connection string from local.settings.json or Azure App Configuration
            string connectionString = Environment.GetEnvironmentVariable("AZURESQL_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Database connection string is not configured.");
            }

            using (SqlConnection connection = new(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new("AddAdd", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = add.Id });
                    command.Parameters.Add(new SqlParameter("@TagText", SqlDbType.NVarChar) { Value = add.TagText });
                    command.Parameters.Add(new SqlParameter("@LogoFile", SqlDbType.NVarChar) { Value = add.LogoFile });
                    command.Parameters.Add(new SqlParameter("@ImageFile", SqlDbType.NVarChar) { Value = add.ImageFile });
                    command.Parameters.Add(new SqlParameter("@IsDiscontinued", SqlDbType.Bit) { Value = add.IsDiscontinued });

                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
