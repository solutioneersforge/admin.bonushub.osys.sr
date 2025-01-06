using Azure.Storage;
using Azure.Storage.Blobs;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Api
{
    public class FunctionStoreImage
    {
        readonly ILogger<FunctionStoreImage> _logger;

        public FunctionStoreImage(ILogger<FunctionStoreImage> logger) => _logger = logger;

        [Function("FunctionStoreImage")]
        public static async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestData req, ILogger log)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var parameters = JsonConvert.DeserializeObject<Add>(requestBody);

            var key = Environment.GetEnvironmentVariable("AZURESTORAGE_ACCOUNT_KEY");
            var account = Environment.GetEnvironmentVariable("AZURESTORAGE_ACCOUNT_NAME");
            var uri = $"https://{account}.blob.core.windows.net";
            StorageSharedKeyCredential sharedKeyCredential = new StorageSharedKeyCredential(account, key);
            var blobServiceClient = new BlobServiceClient(new Uri(uri), sharedKeyCredential);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(account);
            BlobClient blobClient = containerClient.GetBlobClient(parameters.ImageBlobName);

            byte[] imageBytes = Convert.FromBase64String(parameters.ImageAsBase64String);
            using (var stream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(stream, true);
            }

            return new OkObjectResult("File uploaded successfully!");
        }
    }
}
