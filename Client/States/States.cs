namespace Client.States
{
    public class States
    {
        public event Func<Task> OnThemeChanged = null!;
        public int TeacherId { get; set; }
        public async Task ChangeTheme()
        {
            if (OnThemeChanged is not null)
                await OnThemeChanged.Invoke();
        }
    }
}
