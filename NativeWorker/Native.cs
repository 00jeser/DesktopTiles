﻿using System.IO;
using System.Runtime.InteropServices;

namespace NativeWorker;
public static class Native
{
    [DllImport("user32.dll")]
    public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    [DllImport("user32.dll")]
    public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);

    [DllImport("user32.dll")]
    static extern int EnumWindows(EnumWindowsProc1 lpEnumFunc, IntPtr lParam);
    private delegate int EnumWindowsProc1(IntPtr hWnd, int lParam);

    private static IntPtr shellWindowHandle;
    /// <summary>
    /// Called by EnumWindows. Sets <code>shellWindowHandle</code> if a window with class "SHELLDLL_DefView" is found during enumeration.
    /// </summary>
    /// <param name="handle">The handle of the window being enumerated.</param>
    /// <param name="param">The argument passed to <code>EnumWindowsProc</code>; not used in this application.</param>
    /// <returns>Allways returns 1.</returns>
    private static int EnumWindowsProc(IntPtr handle, int param)
    {
        try
        {
            IntPtr foundHandle = FindWindowEx(handle, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (!foundHandle.Equals(IntPtr.Zero))
            {
                shellWindowHandle = foundHandle;
                return 0;
            }
        }
        catch
        {
            // Intentionally left blank
        }

        return 1;
    }

    /// <summary>
    /// Finds the window containing desktop icons.
    /// </summary>
    /// <returns>The handle of the window.</returns>
    public static IntPtr FindShellWindow()
    {
        IntPtr progmanHandle;
        IntPtr defaultViewHandle = IntPtr.Zero;
        IntPtr workerWHandle;
        int errorCode = 0;

        // Try the easy way first. "SHELLDLL_DefView" is a child window of "Progman".
        progmanHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Progman", null);

        if (!progmanHandle.Equals(IntPtr.Zero))
        {
            defaultViewHandle = FindWindowEx(progmanHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            errorCode = Marshal.GetLastWin32Error();
        }

        if (!defaultViewHandle.Equals(IntPtr.Zero))
        {
            return defaultViewHandle;
        }
        else if (errorCode != 0)
        {
            Marshal.ThrowExceptionForHR(errorCode);
        }

        // Try the not so easy way then. In some systems "SHELLDLL_DefView" is a child of "WorkerW".
        errorCode = 0;
        workerWHandle = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "WorkerW", null);

        if (!workerWHandle.Equals(IntPtr.Zero))
        {
            defaultViewHandle = FindWindowEx(workerWHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            errorCode = Marshal.GetLastWin32Error();
        }

        if (!defaultViewHandle.Equals(IntPtr.Zero))
        {
            return defaultViewHandle;
        }
        else if (errorCode != 0)
        {
            Marshal.ThrowExceptionForHR(errorCode);
        }

        shellWindowHandle = IntPtr.Zero;

        // Try the hard way. In some systems "SHELLDLL_DefView" is a child or a child of "Progman".
        if (EnumWindows(EnumWindowsProc, progmanHandle) == 0)
        {
            errorCode = Marshal.GetLastWin32Error();
            if (errorCode != 0)
            {
                Marshal.ThrowExceptionForHR(errorCode);
            }
        }


        return shellWindowHandle;
    }







    [DllImport("user32.dll", SetLastError = true)] static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", SetLastError = true)] static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);
    enum GetWindow_Cmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }
    [DllImport("user32.dll", CharSet = CharSet.Auto)] static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

    private const int WM_COMMAND = 0x111;

    public static void ToggleDesktopIcons()
    {
        var toggleDesktopCommand = new IntPtr(0x7402);
        IntPtr hWnd = GetWindow(FindWindow("Progman", "Program Manager"), GetWindow_Cmd.GW_CHILD);
        SendMessage(hWnd, WM_COMMAND, toggleDesktopCommand, IntPtr.Zero);
    }
}
