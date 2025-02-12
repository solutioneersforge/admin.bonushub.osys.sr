using Blazored.LocalStorage;
using Client;
using Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Azure.Functions.Authentication.WebAssembly;
using Microsoft.JSInterop;
using MudBlazor.Services;
using System.Globalization;
using TextCopy;
using static MudBlazor.Colors;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddStaticWebAppsAuthentication();
builder.Services.AddSessionStorageServices();
builder.Services.InjectClipboard();
builder.Services.AddBlazoredLocalStorage();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    string[] supportedCultures = ["tr", "en"];
    options
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures)
        .SetDefaultCulture("tr");
});

builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources");

var host = builder.Build();

const string defaultCulture = "tr-TR";

var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");
var culture = CultureInfo.GetCultureInfo(result ?? defaultCulture);

if (result == null)
{
    await js.InvokeVoidAsync("blazorCulture.set", defaultCulture);
    var l = host.Services.GetRequiredService<ILocalStorageService>();
    await l.SetItemAsync("blazorCulture", defaultCulture);
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
