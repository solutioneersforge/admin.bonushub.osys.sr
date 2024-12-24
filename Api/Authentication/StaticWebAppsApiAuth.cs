using Microsoft.Azure.Functions.Worker.Http;
using Shared.Authentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Api.Authentication
{
    public class StaticWebAppsApiAuth
    {
        private const string ClientPrincipalHeader = "x-ms-client-principal";

        public static ClaimsPrincipal Parse(HttpRequestData req)
        {
            var clientPrincipal = new ClientPrincipal();

            if (req.Headers.TryGetValues(ClientPrincipalHeader, out IEnumerable<string> headers))
            {
                var header = headers?.FirstOrDefault();
                var decoded = Convert.FromBase64String(header);
                var json = Encoding.UTF8.GetString(decoded);
                clientPrincipal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            return Shared.Authentication.Services.AuthenticationHelper.GetClaimsPrincipalFromClientPrincipal(clientPrincipal);
        }
    }
}
