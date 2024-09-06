
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Controls.CommonDialog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
using ClassIsland.Core;

namespace GrantUiAccess;

[PluginEntrance]
public class Plugin : PluginBase
{
    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        var hr = PrepareForUIAccess();
        Console.WriteLine(hr);
        Marshal.ThrowExceptionForHR(hr);

        AppBase.Current.AppStarted += CurrentOnAppStarted;
    }

    private void CurrentOnAppStarted(object? sender, EventArgs e)
    {
        if (AppBase.Current.MainWindow?.Topmost == true)
        {
            AppBase.Current.MainWindow.Topmost = false;
            AppBase.Current.MainWindow.Topmost = true;
        }
    }

    [DllImport("UIAccessDLL.x64.dll", EntryPoint = "PrepareForUIAccess", CallingConvention = CallingConvention.Cdecl)]
    private static extern int PrepareForUIAccess();
}