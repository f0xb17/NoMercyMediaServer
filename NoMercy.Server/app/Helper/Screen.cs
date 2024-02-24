using System.Runtime.InteropServices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace NoMercy.Server.app.Helper;

public static class Screen
{
    public static int GetScreenWidth()
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