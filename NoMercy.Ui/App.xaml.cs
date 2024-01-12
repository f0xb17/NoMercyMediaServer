using System.Runtime.InteropServices;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace NoMercy.Ui
{
    public partial class App: Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            var width = GetScreenWidth();
            var newWidth = Math.Floor(width / 1.7);
            var newHeight = Math.Floor((newWidth / 16) * 9);

            window.Title = "NoMercy MediaServer";

            window.X = 50;
            window.Y = 50;

            window.Width = newWidth;
            window.Height = newHeight;

            window.MinimumHeight = newHeight;
            window.MinimumWidth = newWidth;

            return window;
        }

        private static int GetScreenWidth()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetScreenWidthWindows() : 1666;
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        private static int GetScreenWidthWindows(int screenIndex = 0)
        {
            return GetSystemMetrics(screenIndex);
        }

    }
}
