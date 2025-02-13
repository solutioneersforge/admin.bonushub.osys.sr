using Data.Models;
using Shared.Enums;
using Shared.Extentions;

namespace Client.States
{
    public class States
    {
        public event Func<Task> OnThemeChanged = null!;
      
        public async Task ChangeTheme()
        {
            if (OnThemeChanged is not null)
                await OnThemeChanged.Invoke();
        }
    }
}
