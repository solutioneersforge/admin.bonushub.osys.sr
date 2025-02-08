using System;

namespace Data.Models;

public class Add
{
    public int Id { get; set; }
    public string AddPlacementZone { get; set; }
    public string LabelText { get; set; }
    public string ImageFileName { get; set; }
    public string RedirectUrl { get; set; }
    public DateTime? ActiveFrom { get; set; }
    public DateTime? ActiveUntil { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public int DisplayOrder { get; set; }
    public int DesktopClickCount { get; set; }
    public int MobileTapCount { get; set; }
    public string User { get; set; }
    public string ImageAsBase64String { get; set; }
    public string ImageBlobName { get; set; }
    public string ImageBlobUrl { get; set; }
    public string BonusAmountText { get; set; }
    public string ImageBgColor { get; set; }
    public string FontFamily { get; set; }
    public string FontSize { get; set; }

}
