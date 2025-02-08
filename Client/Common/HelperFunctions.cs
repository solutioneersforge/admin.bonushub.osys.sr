using MudBlazor;
using Shared.Enums;

namespace Client.Common
{
    internal static class HelperFunctions
    {
        internal static string GetPlacementZoneIcon(PlacementZone slot)
        {
            return slot switch
            {
                PlacementZone.Top => @Icons.Material.Filled.AlignVerticalTop,
                PlacementZone.Center => @Icons.Material.Filled.AlignVerticalCenter,
                PlacementZone.Right => @Icons.Material.Filled.AlignHorizontalRight,
                PlacementZone.Left => @Icons.Material.Filled.AlignHorizontalLeft,
                PlacementZone.Vip => @Icons.Material.Filled.Window,
                _ => @Icons.Material.Filled.DeviceUnknown,
            };
        }
    }
}
