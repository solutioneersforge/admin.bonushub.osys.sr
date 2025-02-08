using Data.Models;
using Shared.Enums;
using Shared.Extentions;

namespace Client.Constants
{
    public class DefaultNewAddItem
    {
        public Add Instance { get; }

        public DefaultNewAddItem(PlacementZone slot)
        {
            Instance = new()
            {
                Id = -1,
                LabelText = "Buraya reklam verebilirsini",
                BonusAmountText = "",
                ImageBgColor = AppColors.Transparent,
                FontFamily = "",
                FontSize = "",
                RedirectUrl = "",
                ImageBlobName = string.Empty,
                ImageFileName = string.Empty,
                ImageAsBase64String = string.Empty,
                AddPlacementZone = slot.GetDescription(),
                IsActive = true,
                IsPublished = false,
                DisplayOrder = -1,
                ActiveFrom = DateTime.Now,
                ActiveUntil = DateTime.Now,
                DesktopClickCount = 0,
                MobileTapCount = 0
            };
        }
    }
}
