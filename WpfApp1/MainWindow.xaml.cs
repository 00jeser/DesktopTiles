using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Link
        {
            public ImageSource image
            {
                get
                {
                    try
                    {
                        return DefaultIcons.ExtractIconFromPath1(uri);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            public string name
            {
                get
                {
                    if (fileName.Contains("."))
                        return string.Join(".", fileName.Split('\\').Last().Split('.').Reverse().Skip(1).Reverse());
                    return fileName.Split('\\').Last();
                }
            }
            public string uri
            {
                get
                {
                    return fileName;
                }
            }
            public string fileName;
        }


        public MainWindow()
        {
            try
            {
                InitializeComponent();
                RunPeriodicSave();
                //F();
                SetOnDesktop(this);
            }
            catch (Exception)
            {

            }
        }
        int a = 0;

        public async Task F()
        {
            while (true)
            {
                await Task.Delay(1000);
                SetBottom(this);
            }
        }
        public async Task RunPeriodicSave()
        {
            // while (true)
            // {
            //     await Task.Delay(1000);
            //     WindowState = WindowState.Normal;
            //     SetOnDesktop(this);
            //     await Task.Delay(1000);
            //     this.Height = 101;
            //     break;
            // }
            await Task.Delay(1000);

            ShowDesktop.AddHook(this);
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hWnd, GWL_EX_STYLE, (GetWindowLong(hWnd, GWL_EX_STYLE) | WS_EX_TOOLWINDOW) & ~WS_EX_APPWINDOW);
            SetBottom(this);
            while (true)
            {
                await Task.Delay(1000);
                if (GetFiles().ToList().Count != a)
                {
                    ls.ItemsSource = GetFiles().ToList().Select(x => new Link() { fileName = x });
                    a = GetFiles().ToList().Count;
                }
            }
        }

        public static IEnumerable<string> GetFiles()
        {
            foreach (string s in Directory.GetDirectories("C:\\Users\\00jes\\OneDrive\\Документы\\Desktop"))
                yield return s;
            foreach (string s in Directory.GetFiles("C:\\Users\\00jes\\OneDrive\\Документы\\Desktop"))
                if (!s.EndsWith("desktop.ini"))
                    yield return s;
            //foreach (string s in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
            //    yield return s;
            //foreach (string s in Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory)))
            //    yield return s;
            //foreach (string s in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)))
            //    if (!s.EndsWith("desktop.ini"))
            //        yield return s;

        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        private const int GWL_EX_STYLE = -20;
        private const int WS_EX_APPWINDOW = 0x00040000, WS_EX_TOOLWINDOW = 0x00000080;


        public static void SetOnDesktop(Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            IntPtr hWndProgMan = GetDesktopWindow(DesktopWindow.ProgMan);
            SetParent(hWnd, hWndProgMan);
        }

        public static void Run(string s)
        {
            System.Diagnostics.Process.Start(s);
        }


        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
        int Y, int cx, int cy, uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        public static void SetBottom(Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }

        private void Window_GotFocus(object sender, RoutedEventArgs e)
        {
            SetBottom(this);
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDrop.DoDragDrop(sender as Button, (sender as Button).Tag, DragDropEffects.Copy);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SetBottom(this);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OpenInExplorer_Click(object sender, RoutedEventArgs e)
        {
            Run(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete((sender as MenuItem).Tag.ToString());
            }
            catch (Exception)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Run((sender as Button).Tag.ToString());
            }
            catch (Exception)
            {

            }
        }




        [DllImport("user32.dll")]
        static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr hWndChildAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        static extern bool EnumWindows(ControlzEx.Standard.NativeMethods.EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll")]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        public enum DesktopWindow
        {
            ProgMan,
            SHELLDLL_DefViewParent,
            SHELLDLL_DefView,
            SysListView32
        }


        static IntPtr _ProgMan;
        static IntPtr _SHELLDLL_DefViewParent;
        static IntPtr _SHELLDLL_DefView;
        static IntPtr _SysListView32;
        public static IntPtr GetDesktopWindow(DesktopWindow desktopWindow)
        {
            _ProgMan = GetShellWindow();
            _SHELLDLL_DefViewParent = _ProgMan;
            _SHELLDLL_DefView = FindWindowEx(_ProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
            _SysListView32 = FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");

            if (_SHELLDLL_DefView == IntPtr.Zero)
            {
                EnumWindows(finding, IntPtr.Zero);
            }

            switch (desktopWindow)
            {
                case DesktopWindow.ProgMan:
                    return _ProgMan;
                case DesktopWindow.SHELLDLL_DefViewParent:
                    return _SHELLDLL_DefViewParent;
                case DesktopWindow.SHELLDLL_DefView:
                    return _SHELLDLL_DefView;
                case DesktopWindow.SysListView32:
                    return _SysListView32;
                default:
                    return IntPtr.Zero;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public static bool finding(IntPtr hwnd, IntPtr lParam)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                int r = GetClassName(hwnd, sb, 2048);
                if (sb.ToString() == "WorkerW")
                {
                    IntPtr child = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                    if (child != IntPtr.Zero)
                    {
                        _SHELLDLL_DefViewParent = hwnd;
                        _SHELLDLL_DefView = child;
                        _SysListView32 = FindWindowEx(child, IntPtr.Zero, "SysListView32", "FolderView"); ;
                        return false;
                    }
                }
            }
            catch (Exception)
            {

            }
            return true;
        }
    }
}
