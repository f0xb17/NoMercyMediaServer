using System.Drawing;
using System.Runtime.Versioning;
using H.NotifyIcon.Core;

namespace NoMercy.Server;

[SupportedOSPlatform("windows10.0.18362")]
public class TrayIcon
{
    private static readonly string IconStream = Path.Combine(Directory.GetCurrentDirectory(), "Assets/icon.ico");
    
    private static readonly Icon Icon = new(IconStream);
    
    private readonly TrayIconWithContextMenu _trayIcon = new(){
        Icon = Icon.Handle,
        ToolTip = "NoMercy MediaServer C#",
    };

    public TrayIcon()
    {
        _trayIcon.ContextMenu = new PopupMenu
        {
            Items =
            {
                new PopupMenuItem("Show App", (_, _) => Show()),
                // new PopupSubMenu("Mistaa FILLYBILLY is?")
                // {
                //     Items =
                //     {
                //         new PopupMenuItem("A: Gek", (_, _) => Show()),
                //         new PopupMenuItem("B: Vliegtuig", (_, _) => Show()),
                //        
                //     }
                // },
                new PopupMenuSeparator(),
                new PopupMenuItem("Pause Server", (_, _) => Pause()),
                new PopupMenuItem("Restart Server", (_, _) => Restart()),
                new PopupMenuItem("Shutdown", (_, _) => Shutdown()),
            },
        };
        
        _trayIcon.Create();
    }

    private void Pause()
    {
        
    }
    private void Show()
    {
        
    }
    
    private void Restart()
    {
        
    }

    private void Shutdown()
    {
        _trayIcon.Dispose();
        Environment.Exit(0);
    }

}