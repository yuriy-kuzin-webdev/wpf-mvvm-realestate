using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RealEstateApp.Services.WinApi
{
    public static class WinApiMessageBox
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr hWnd, String text, String caption, uint type);
        public static bool ConfirmAction(string text)
        {
            return MessageBox(Process.GetCurrentProcess().MainWindowHandle, text, "Подтверждение действия", 1) == 1;
        }
    }
}
