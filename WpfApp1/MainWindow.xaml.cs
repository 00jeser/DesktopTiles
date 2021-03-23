using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
                        if (fileName.IndexOf(".lnk") != -1)
                        {
                            var shl = new Shell32.Shell();
                            fileName = System.IO.Path.GetFullPath(fileName);
                            var dir = shl.NameSpace(System.IO.Path.GetDirectoryName(fileName));
                            var itm = dir.Items().Item(System.IO.Path.GetFileName(fileName));
                            var lnk = (Shell32.ShellLinkObject)itm.GetLink;
                            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(lnk.Target.Path);
                            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                                icon.Handle,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions()
                                );
                            return imageSource;
                        }
                        else
                        {
                            Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(fileName);
                            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                                icon.Handle,
                                Int32Rect.Empty,
                                BitmapSizeOptions.FromEmptyOptions()
                                );
                            return imageSource;
                        }
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
                    return fileName.Split('\\').Last().Split('.').First();
                }
            }
            public string fileName;
        }


        public MainWindow()
        {
            try
            {
                InitializeComponent();
                ls.ItemsSource = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)).ToList().Select(x => new Link() { fileName = x });
                RunPeriodicSave();
            }
            catch (Exception)
            {

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

            ShowDesktop.AddHook(this);
            await Task.Delay(1000);
            SetBottom(this);
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void SetOnDesktop(Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            IntPtr hWndProgMan = FindWindow("ProgMan", null);
            SetParent(hWnd, hWndProgMan);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
