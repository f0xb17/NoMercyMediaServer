using ElectronNET.API;
using ElectronNET.API.Entities;

namespace NoMercy.Server.Helpers;

public static class ElectronWindows
{
    private static BrowserWindow _mainWindow = null!;
    private static BrowserWindow _splashScreen = null!;
    private static BrowserWindow _dummyScreen = null!;
    private static readonly int Width = Screen.GetScreenWidth();

    static ElectronWindows()
    {
        if (!HybridSupport.IsElectronActive) return;
        
        Electron.App.CommandLine.AppendSwitch("ignore-certificate-errors");
        Electron.App.CommandLine.AppendSwitch("disable-web-security");
        
        Electron.WindowManager.IsQuitOnWindowAllClosed = false;
        Electron.App.BeforeQuit += AppOnBeforeQuit;
        Electron.App.WindowAllClosed += AppOnWindowAllClosed;
        Electron.App.WillQuit += AppOnWillQuit;
    }
    
    public static async Task Start()
    {
        if (!HybridSupport.IsElectronActive) await Task.CompletedTask;
        
        TrayIcon();
        
        await DummyScreen();
        await SplashScreen();

        Task.Delay(5000).Wait();
        await MainWindow();
    }


    private static Task<bool> AppOnWillQuit(QuitEventArgs arg)
    {
        arg.PreventDefault();
        return Task.FromResult(false);
    }

    private static void AppOnWindowAllClosed()
    {
        // Electron.App.Relaunch();
    }

    private static Task<bool> AppOnBeforeQuit(QuitEventArgs arg)
    {
        arg.PreventDefault();
        return Task.FromResult(false);
    }

    private static async Task MainWindow()
    {
        _mainWindow = await Electron.WindowManager.CreateWindowAsync();
        
        _mainWindow.SetParentWindow(_dummyScreen);
        
        _splashScreen.Hide();
        
        _mainWindow.LoadURL("https://vue-dev2.nomercy.tv");
        
        var h = Math.Floor(Width / 1.1);
        var w = Math.Floor(h / (16 * 9));
        
        _mainWindow.OnReadyToShow += () =>
        {
            _mainWindow.SetSize((int)h, (int)w);
            _mainWindow.SetMinimumSize(1320, 860);
            _mainWindow.SetResizable(true);
            _mainWindow.SetMaximizable(true);
            _mainWindow.Center();
            _mainWindow.SetTitle("NoMercy MediaServer C#");
            _mainWindow.SetMenuBarVisibility(false);
            _mainWindow.SetClosable(true);
            _mainWindow.SetResizable(true);
        };
        
        _mainWindow.WebContents.OnDidFinishLoad += () =>
        {
            _mainWindow.Show();
            
            _mainWindow.OnClose += () => _mainWindow.Hide();
        };
    }

    private static async Task DummyScreen()
    {
        _dummyScreen = await Electron.WindowManager.CreateWindowAsync(
            new BrowserWindowOptions
            {
                Width = 0,
                Height = 0,
                Show = false,
                Resizable = false,
                Maximizable = false,
                Center = true,
                Frame = true,
                Icon = AppFiles.AppIcon,
                WebPreferences = WebPreferences
            }
        );
        
    }

    private static async Task SplashScreen()
    {
        _splashScreen = await Electron.WindowManager.CreateWindowAsync();
        _splashScreen.SetParentWindow(_dummyScreen);
        
        _splashScreen.LoadURL("https://cdn.nomercy.tv/splash.html");

        _splashScreen.OnReadyToShow += () =>
        {
            _splashScreen.SetSize(600, 300);
            _splashScreen.Center();
            _splashScreen.SetTitle("NoMercy MediaServer C#");
            _splashScreen.SetMenuBarVisibility(false);
            _splashScreen.SetClosable(true);
            _splashScreen.SetResizable(true);
            _splashScreen.Show();
        };
    }

    private static async void TrayIcon()
    {
        var menu = new[]
        {
            new MenuItem
            {
                Label = "HomePage",
                Click = () =>
                    Electron
                        .WindowManager.BrowserWindows.First()
                        .LoadURL("https://nomercy.tv")
            },
            new MenuItem
            {
                Label = "Show Window",
                Click = () => _mainWindow.Show(),
            },
            new MenuItem
            {
                Label = "Exit",
                Click = () => Electron.App.Exit()
            }
        };

        await Electron.Tray.Show(AppFiles.AppIcon, menu);
        await Electron.Tray.SetToolTip("NoMercy MediaServer C#");

        Electron.Tray.OnDoubleClick += (_,_) => _mainWindow.Show();
    }
    
    private static WebPreferences WebPreferences =>
        new()
        {
            NodeIntegration = false,
            WebSecurity = false,
            ContextIsolation = false,
            Webgl = true,
        };

}