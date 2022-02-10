using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Avalonia.Win32;
using NativeWorker;

namespace AvaloniaApplication1.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            AttachToDesktop();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void AttachToDesktop()
        {
            IntPtr handle = IntPtr.Zero;
            while (handle == IntPtr.Zero)
            {
                var toplevel = (TopLevel)this.GetVisualRoot();

                var platformImpl = (WindowImpl)toplevel.PlatformImpl;

                handle = platformImpl.Handle.Handle;
                await Task.Delay(1000);
            }
            Native.SetParent(handle, Native.FindShellWindow());
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
        }
    }
}
