
using System.ComponentModel;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Threading;
using ClassIsland.Core;
using System.Linq;
using System.Text;

namespace GrantUiAccess;

[PluginEntrance]
public class Plugin : PluginBase
{
    public override void Initialize(HostBuilderContext context, IServiceCollection services)
    {
        var hr = PrepareForUIAccess();
        Console.WriteLine(hr);
        if (hr != 0)
        {
            throw new Win32Exception(hr);
        }

        AppBase.Current.AppStarted += CurrentOnAppStarted;
    }

    private void CurrentOnAppStarted(object? sender, EventArgs e)
    {
        _ = Dispatcher.UIThread.InvokeAsync(() =>
        {
            if (AppBase.Current.MainWindow == null)
            {
                return;
            }

            if (AppBase.Current.MainWindow.Topmost != true)
                return;
            AppBase.Current.MainWindow.Topmost = false;
            AppBase.Current.MainWindow.Topmost = true;

            FixTopmostEffectWindow();
        });
    }

    private static void FixTopmostEffectWindow()
    {
        var windows = AppBase.Current?.GetType()
            .GetProperty("Windows", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public)
            ?.GetValue(AppBase.Current) as System.Collections.IEnumerable;

        if (windows == null)
            return;

        foreach (var windowObj in windows)
        {
            if (windowObj is not Window window)
                continue;

            if (window.GetType().Name != "TopmostEffectWindow")
                continue;

            if (!window.IsVisible)
                continue;

            window.Topmost = false;
            window.Topmost = true;

            var handle = window.TryGetPlatformHandle()?.Handle ?? nint.Zero;
            if (handle == nint.Zero)
                continue;

            var exStyle = GetWindowLong(handle, GWL_EXSTYLE);
            if ((exStyle & WS_EX_TOOLWINDOW) == 0)
            {
                SetWindowLong(handle, GWL_EXSTYLE, exStyle | WS_EX_TOOLWINDOW);
                SetWindowPos(handle, HWND_TOPMOST, 0, 0, 0, 0,
                    SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE | SWP_FRAMECHANGED);
            }
        }
    }

    [DllImport("uiaccess.dll", EntryPoint = "PrepareForUIAccess", CallingConvention = CallingConvention.Cdecl)]
    private static extern int PrepareForUIAccess();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPos(nint hWnd, nint hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_TOOLWINDOW = 0x00000080;
    private static readonly nint HWND_TOPMOST = new(-1);
    private const uint SWP_NOSIZE = 0x0001;
    private const uint SWP_NOMOVE = 0x0002;
    private const uint SWP_NOACTIVATE = 0x0010;
    private const uint SWP_FRAMECHANGED = 0x0020;
}
