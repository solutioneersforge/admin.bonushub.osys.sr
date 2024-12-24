using Client.Constants;
using Microsoft.AspNetCore.Components;
using MudBlazor;
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

        private readonly Action<SnackbarOptions> config = (SnackbarOptions options) =>
        {
            options.DuplicatesBehavior = SnackbarDuplicatesBehavior.Prevent;
            options.ShowCloseIcon = true;
        };

        private static System.Timers.Timer timerGreeting = new();
        private int counterGreeting = 0;
        private bool ShowAlertGreeting = true;
        private string GreetingText = string.Empty;
        private void CloseAlertGreeting() => ShowAlertGreeting = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
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

                await Task.CompletedTask;
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
                SnackbarService.Add(ex.Message, Severity.Error, configure: config);
                return;
            }
        }

        private static string GenerateGreetingText()
        {
            var today = DateTime.Now;
            return today.Hour switch
            {
                >= 5 and < 12 => "Good morning",
                >= 12 and < 18 => "Good afternoon",
                _ => "Good evening"
            };
        }
    }
}
