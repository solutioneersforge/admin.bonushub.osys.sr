using MudBlazor;

namespace Client.Constants
{
    public class StandardTheme
    {
        public const string FontFamily = "Open Sans";
        private static readonly string[] fontFamily = [FontFamily];
        private static readonly Typography typography = new()
        {
            Default = new Default { FontFamily = fontFamily },
            Overline = new Overline { FontFamily = fontFamily },
            Body1 = new Body1 { FontFamily = fontFamily },
            Body2 = new Body2 { FontFamily = fontFamily },
            Button = new Button { FontFamily = fontFamily },
            H1 = new H1 { FontFamily = fontFamily },
            H2 = new H2 { FontFamily = fontFamily },
            H3 = new H3 { FontFamily = fontFamily },
            H4 = new H4 { FontFamily = fontFamily },
            H5 = new H5 { FontFamily = fontFamily },
            H6 = new H6 { FontFamily = fontFamily },
            Caption = new Caption { FontFamily = fontFamily },
            Subtitle1 = new Subtitle1 { FontFamily = fontFamily },
            Subtitle2 = new Subtitle2 { FontFamily = fontFamily },
        };
        private static readonly PaletteLight lightThemePallete = new()
        {
            DrawerBackground = "#ffffffcc",
            Surface = "#ffffffcc", // MudDialog uses this as a background
            AppbarBackground = "#594ae2cc",

            Primary = AppColors.Tuna,
            Secondary = AppColors.Orange,
            Tertiary = AppColors.Purple,
            Info = AppColors.Blue,
            Success = AppColors.Green,
            Warning = AppColors.Yellow,
            Error = AppColors.Red,
            Dark = AppColors.Tuna,
            Black = AppColors.Tuna,
            Background = AppColors.White,
            TextPrimary = AppColors.Tuna,
            TextSecondary = AppColors.Tuna,
            AppbarText = AppColors.Tuna,
            DrawerText = AppColors.Tuna,
            DrawerIcon = AppColors.Tuna,
            DarkContrastText = AppColors.White,
        };

        private static readonly PaletteDark darkThemePallete = new()
        {
            DrawerBackground = "#27272fcc",
            Surface = "#373740cc",
            AppbarBackground = "#27272fcc",

            Primary = AppColors.White,
            Secondary = AppColors.Orange,
            Tertiary = AppColors.Purple,
            Info = AppColors.Blue,
            Success = AppColors.Green,
            Warning = AppColors.Yellow,
            Error = AppColors.Red,
            Dark = AppColors.Tuna,
            Black = AppColors.Tuna,
            Background = AppColors.Tuna,
            TextPrimary = AppColors.White,
            TextSecondary = AppColors.White,
            AppbarText = AppColors.White,
            DrawerText = AppColors.White,
            DrawerIcon = AppColors.White,
            DarkContrastText = AppColors.White,
        };

        public static bool IsInDarkMode { get; set; } = false;
        public static MudTheme CurrentTheme { get; set; } = LightTheme ?? new()
        {
            PaletteLight = lightThemePallete,
            PaletteDark = darkThemePallete,
            Typography = typography
        };

        public static MudTheme LightTheme { get; set; } =
        new()
        {
            PaletteLight = lightThemePallete,
            PaletteDark = darkThemePallete,
            Typography = typography
        };

        public static MudTheme DarkTheme { get; set; } =
        new()
        {
            PaletteLight = lightThemePallete,
            PaletteDark = darkThemePallete,
            Typography = typography
        };

    }
}
