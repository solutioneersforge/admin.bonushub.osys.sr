using Microsoft.AspNetCore.Components;

namespace Client.LoadingIndicators.svg;

public class BaseSvgLoading : ComponentBase
{
    [Parameter] public int Size { get; set; } = 200;
}
