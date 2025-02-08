using Data.Models;
using Shared.Enums;
using Shared.Extentions;

namespace Client.States
{
    public class States
    {
        public event Func<Task> OnThemeChanged = null!;
        public List<AllowedAddsRange> AllowedAddsRangePerSlot { get; set; } = [new AllowedAddsRange { Slot = PlacementZone.Vip.GetDescription(), Minimum = 0, Maximum = 30 },
        new AllowedAddsRange { Slot = PlacementZone.Top.GetDescription(), Minimum = 0, Maximum = 10 },
        new AllowedAddsRange { Slot = PlacementZone.Center.GetDescription(), Minimum = 0, Maximum = 10 },
        new AllowedAddsRange { Slot = PlacementZone.Right.GetDescription(), Minimum = 0, Maximum = 10 },
        new AllowedAddsRange { Slot = PlacementZone.Left.GetDescription(), Minimum = 0, Maximum = 10 }];

        public async Task ChangeTheme()
        {
            if (OnThemeChanged is not null)
                await OnThemeChanged.Invoke();
        }
    }
}
