using System.ComponentModel;

namespace Shared.Enums
{
    public enum Settings
    {
        [Description("ImageDimensions")]
        ImageDimensions,
        [Description("ImageMaxFilesize")]
        ImageMaxFilesize,
        [Description("AllowedAddsRangePerSlot")]
        AllowedAddsRangePerSlot, 
        [Description("AzureStorageContainerPath")]
        AzureStorageContainerPath,
    }
}
