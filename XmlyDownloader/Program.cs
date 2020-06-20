using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Logging.Serilog;
using Avalonia.ReactiveUI;

namespace XmlyDownloader
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp()
            .With(new AvaloniaNativePlatformOptions { UseGpu = false })
            .With(new MacOSPlatformOptions { ShowInDock = false })
            .With(new Win32PlatformOptions { UseDeferredRendering = false })
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();
    }
}
