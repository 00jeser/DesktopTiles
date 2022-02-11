using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using Avalonia.Win32;
using AvaloniaApplication1.Controls;
using AvaloniaApplication1.Models;
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

            ObservableCollection<ObservableCollection<Tile>> tiles = new();
            for (int i = 0; i < 4; i++)
            {
                tiles.Add(new ObservableCollection<Tile>());
                tiles.Last().Add(new TileItem());
                tiles.Last().Add(new TileItem());
                tiles.Last().Add(new TileItem());
                tiles.Last().Add(new TileItem());
                tiles.Last().Add(new TileFolder());
            }
            this.FindControl<ItemsControl>("tiles").Items = tiles;
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
            int xPos = -Screens.All.Select(x => x.Bounds.X).Min() - 1;
            int yPos = -Screens.All.Select(x => x.Bounds.Y).Min() - 1;
            var win = Screens.Primary.Bounds;
            Position = new PixelPoint(xPos, yPos);
            Width = win.Width - 2;
            Height = win.Height - 2;
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
        }

        private void Row_OnFolderOpenEvent(Row sender)
        {

        }
    }
}
