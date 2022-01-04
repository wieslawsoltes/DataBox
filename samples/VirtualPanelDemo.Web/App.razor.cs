using Avalonia.ReactiveUI;
using Avalonia.Web.Blazor;

namespace VirtualPanelDemo.Web;

public partial class App
{
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        WebAppBuilder.Configure<VirtualPanelDemo.App>()
            .UseReactiveUI()
            .SetupWithSingleViewLifetime();
    }
}
