using MudBlazor;

namespace Client.Common
{
    internal static class HelperFunctions
    {
        internal static string GetPlacementZoneIcon(string loc)
        {
            return loc switch
            {
                "Top" => @Icons.Material.Filled.VerticalAlignTop,
                "Bottom" => @Icons.Material.Filled.VerticalAlignBottom,
                "Middle" => @Icons.Material.Filled.VerticalAlignCenter,
                "Right" => @Icons.Material.Filled.AlignHorizontalRight,
                "Left" => @Icons.Material.Filled.AlignHorizontalLeft,
                "Popup" => @Icons.Material.Filled.Window,
                _ => @Icons.Material.Filled.DeviceUnknown,
            };
        }
    }
}
