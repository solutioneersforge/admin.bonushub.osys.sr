using System.ComponentModel;

namespace Shared.Enums
{
    public enum PlacementZone
    {
        [Description("Left")]
        Left = 1,
        [Description("Middle")]
        Middle = 2,
        [Description("Popup")]
        Popup = 3,
        [Description("Right")]
        Right = 4,
        [Description("Top")]
        Top = 5,
    }
}
