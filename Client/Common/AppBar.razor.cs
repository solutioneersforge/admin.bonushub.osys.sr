using Client.Constants;
using Data.AzureFunctionResponse;
using Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using Shared.Enums;
using Shared.Extentions;
using System.Globalization;
using System.Text.Json;
using ComponentBase = Microsoft.AspNetCore.Components.ComponentBase;

namespace Client.Common
{
    public partial class AppBar : ComponentBase
    {
        [Parameter]
        public EventCallback OnSideBarToggled { get; set; }

        [Parameter]
        public EventCallback<MudTheme> OnThemeToggled { get; set; }

        [Parameter]
        public bool SideBarOpen { get; set; }
        public const string TurkishFlag = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\">\r\n                    <image xlink:href=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAASlJREFUSEtjZKAxYKSx+QzDwIIbDOr/aRlMjKMWEApenEHEJMDLwOZsycAsKsjw69Rlht/nrhEyC6s8VgtYDTQYBFf0Mvz7+JnhS8cchr/3HjP8efCM4f/HzyRbgmkBExODyMkVDIx8PAxvLSMZ/r37CDeUzVyP4feFGwwM//4x/P/9hyjLMCxgUZVnEDm9iuHb3LUMn4q74IYwiwkx8NRmMrBa6DN8Lu9l+LnvJJkWqMgxiJxZzfBt/nqGT4Ud2C0o62H4uf8UeRYwMDJCgkiADxJEbz9gBtH//wz/f/0m0wIGBgZwJC/vYfj3+SvDl865DH/vPmL4c/8plSIZ6i5QJLO7WkGS6clLDL/PXyfKxeiKRosKgsFG+yAi6AQKFQyDKpPCECCoHQDYb335dyjx/gAAAABJRU5ErkJggg==\" x=\"0\" y=\"0\" width=\"24\" height=\"24\"/>\r\n                  </svg>";

        public const string EnglishFlag = "<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\">\r\n                    <image xlink:href=\"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAAAXNSR0IArs4c6QAAAzxJREFUSEvdlWtMk1cYx3/n7UXtWkOpSAEXxM0wL9kkomkmJMt0zATJJhPoIARjYKJmuCXLrBu6DyK6zShhbkOniYoS0egXqbe5REJUEjUgulgxtTIuyhAotVwGtO+S13hpILFb0i+ej+c5z/PL+T/P+R9BiJcIcX1eAcANS7bss60hfMEsys60UfF7B0Mj/gDlylfOJP3yKbzbf1P29bYCTr6bRtGBu+Mq/N7sMLYlSkRV7EfcTMyUNQ23eLwshbBNhfwTbqTkZAvVV/5Glp/kBwuIj9axOVlPUs1xBqvsaC1vI/J3X5dz+Atz+R6k1g56Mj5m2uYCmj1QXO3iyl3PSwEmgwZbSiTW5loGdh5EFWlCX1yIOu19BOk1sk4rsT4lhqXNlzHvO4isUtG1KpdZX66g5oYb/UQVFvuJMRJtqHJSuNhMkexkdMtuZO8Ar63PRbfGSq1zgG+O3nsCeCpktFHLhg8iSLxgx3T4GP4YM71FnzEnbwne7fsCAK252RgbGlF//yu+Oy50q9LRbyygyS2Um9c5+pSy4mJj2zPAU9D0iImMdHbjcT5QtmIs8Yz+fCQAoP8ilxGHS4lLURGKLD6/zJ9tA/j9z0uKNtuOMYDxRmO47hrDlxqUkHZRAtrkxKDeqHAQHxQgqGrjHAo9IOQSvdhkSRLMmaZDJQl8nd34H3Qpl9a8FYe3rDKgyep1ObTX31Hik9+IQhNp4n7X0BiRno3pwjcnU5I5nQVRGvp/OoK37BDq2GhGvy6k17KQ1yurAqfIlo+juhZD2V7Uzhbcy1O5nZHFd+d7aHn0HCRmrv1D/nZ5LB8lGBk8fIr+0j3IksSEr/I5YJ7Pj6fbKbXOGONF9amf4B3ykZZgxLH/DKbyvYj+QbqyM7iU9CFb7R14Bn2IR929sr/uKp6Nu/Ddb2fS6izOJS9jk72Th+7hl3rR/BkGSrLieGeqBtcPlUw5dBT/lHAe5udRFTYbcW+RVR6ub2JSdio3Mz/FdqGPW639/9lNl84Lp9Qah8HTh3tLBYaa84wkzEW4FufJ3Z8XUNwkcbaxZ9xxD9ZNNSpBTlIkxemxPG5sRtr2y6vwo/1fCwg2L+Sf/r8CTnWEBLzOOgAAAABJRU5ErkJggg==\" x=\"0\" y=\"0\" width=\"24\" height=\"24\"/>\r\n                  </svg>";

        private readonly Action<SnackbarOptions> config = (SnackbarOptions options) =>
        {
            options.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
            options.ShowCloseIcon = true;
        };

        private static System.Timers.Timer timerGreeting = new();
        private int counterGreeting = 0;
        private bool ShowAlertGreeting = true;
        private bool IsLoadingSettings;
        private string GreetingText = string.Empty;
        string LoggedInUser = string.Empty;
        CultureInfo SelectedCultureInfo = CultureInfo.CurrentCulture;
        private void CloseAlertGreeting() => ShowAlertGreeting = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await GetSelectedCultureInfo();

                //GreetingText = $"{GenerateGreetingText()}, you are logged on as {AppVars.CurrentWindowsUserAccount.ToLower()}";
                GreetingText = $"{GenerateGreetingText()}";

                StateHasChanged();

                timerGreeting = new System.Timers.Timer(5000);
                timerGreeting.Elapsed += async (o, e) =>
                {
                    if (counterGreeting > 0)
                    {
                        counterGreeting -= 1;
                    }
                    else
                    {
                        timerGreeting.Stop();
                        ShowAlertGreeting = false;
                        await InvokeAsync(StateHasChanged);
                    }
                };
                timerGreeting.Start();

#if DEBUG
                await LoadSettings();
#endif

#if !DEBUG
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                if (user.Identity != null && user.Identity.IsAuthenticated)
                {
                    LoggedInUser = user.Identity?.Name ?? string.Empty;

                    if (string.IsNullOrEmpty(LoggedInUser)) return;

                    await LoadSettings();
                }
#endif
            }
            catch
            {
                //do not show message
            }
            finally
            {
                //ApplicationState.IsAppBarLoadCompleted = true; ShowDbConnectionAlert = true; LoadingProgressMessage = string.Empty;
            }
        }

        async Task LoadSettings()
        {
            try
            {
                IsLoadingSettings = true;
                var func = AzureFunctions.GetAllowedAddsRanges.GetName();
                var response = await Http.GetStringAsync($"{func}");

                var data = JsonSerializer.Deserialize<AfrAllowedAddsRange>(response);
                var ranges = data?.Value.ToList() ?? [];
                await LocalStorage.SetItemAsStringAsync(Settings.AllowedAddsRangePerSlot.GetDescription(), JsonSerializer.Serialize(ranges));

                func = AzureFunctions.GetSettingsByName.GetName();

                response = await Http.GetStringAsync($"{func}/{Settings.ImageDimensions.GetDescription()}");
                var settingData = JsonSerializer.Deserialize<AfrSetting>(response);
                var settingValues = settingData?.Value.ToList() ?? [];
                var setting = settingValues.FirstOrDefault();
                if (setting is not null)
                {
                    var imageDimension = JsonSerializer.Deserialize<ImageDimension>(setting.SettingValue) ?? new ImageDimension { MinHeight = 150, MinWidth = 150, MaxHeight = 200, MaxWidth = 200 };
                    await LocalStorage.SetItemAsStringAsync(Settings.ImageDimensions.GetDescription(), JsonSerializer.Serialize(imageDimension));
                }

                response = await Http.GetStringAsync($"{func}/{Settings.ImageMaxFilesize.GetDescription()}");
                settingData = JsonSerializer.Deserialize<AfrSetting>(response);
                settingValues = settingData?.Value.ToList() ?? [];
                setting = settingValues.FirstOrDefault();
                if (setting is not null)
                {
                    var imageMaxFilesize = JsonSerializer.Deserialize<int>(setting.SettingValue);
                    await LocalStorage.SetItemAsStringAsync(Settings.ImageMaxFilesize.GetDescription(), JsonSerializer.Serialize(imageMaxFilesize));
                }

                response = await Http.GetStringAsync($"{func}/{Settings.AzureStorageContainerPath.GetDescription()}");
                settingData = JsonSerializer.Deserialize<AfrSetting>(response);
                settingValues = settingData?.Value.ToList() ?? [];
                setting = settingValues.FirstOrDefault();
                if (setting is not null)
                {
                    await LocalStorage.SetItemAsStringAsync(Settings.AzureStorageContainerPath.GetDescription(), setting.SettingValue);
                }
            }
            catch (Exception ex)
            {
                SnackbarService.Clear();
                SnackbarService.Add(ex.Message, Severity.Error, configure: config);
            }
            finally
            {
                IsLoadingSettings = false;
            }
        }

        private async Task ToggleTheme()
        {
            try
            {
                StandardTheme.IsInDarkMode = !StandardTheme.IsInDarkMode;
                StandardTheme.CurrentTheme = StandardTheme.IsInDarkMode ? StandardTheme.DarkTheme : StandardTheme.LightTheme;
                await OnThemeToggled.InvokeAsync(StandardTheme.CurrentTheme);
            }
            catch (Exception ex)
            {
                SnackbarService.Clear();
                SnackbarService.Add(ex.Message, Severity.Error, configure: config);
            }
        }

        private string GenerateGreetingText()
        {
            var today = DateTime.Now;
            return today.Hour switch
            {
                >= 5 and < 12 => Localizer["Good morning"],
                >= 12 and < 18 => Localizer["Good afternoon"],
                _ => Localizer["Good evening"]
            };
        }


        async Task SwitchLanguage()
        {
            var cultures = LocalizationOptions.Value.SupportedCultures;
            var name = await LocalStorage.GetItemAsync<string>("blazorCulture");
            var value = cultures.FirstOrDefault(x => x.Name == name) ?? cultures[0];
            if (CultureInfo.CurrentCulture != value)
            {
                value = value == cultures[0] ? cultures[1] : cultures[0];
                await JS.InvokeVoidAsync("blazorCulture.set", value.Name);
                await LocalStorage.SetItemAsync("blazorCulture", value.Name);
            }

            NavManager.NavigateTo(NavManager.Uri, forceLoad: true);
        }

        async Task GetSelectedCultureInfo()
        {
            var result = await LocalStorage.GetItemAsync<string>("blazorCulture");
            SelectedCultureInfo = result != null ? new CultureInfo(result) : new CultureInfo("en-US");
        }
    }
}
