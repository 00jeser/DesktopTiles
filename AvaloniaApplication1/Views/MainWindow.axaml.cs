using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using Avalonia.VisualTree;
using Avalonia.Win32;
using DesktopTiles.Controls;
using DesktopTiles.Models;
using DesktopTiles.Utils;
using DynamicData;
using Microsoft.Win32;
using NativeWorker;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using Size = System.Drawing.Size;

namespace DesktopTiles.Views
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Tile> tiles = new();
        public MainWindow()
        {
            WindowInfo.Width = Screens.Primary.Bounds.Width;
            WindowInfo.Height = Screens.Primary.Bounds.Height;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            //Styles.Add(AvaloniaRuntimeXamlLoader.Parse<Styles>(File.ReadAllText("D:\\Programming\\vs\\DesktopTiles\\AvaloniaApplication1\\Styles\\Win11.axaml")));
            this.FindControl<ItemsControl>("tiles").Items = tiles;
            AttachingToDesktop();
            WindowInfo.MainWindow = this;
            AddTiles();
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async void AttachingToDesktop()
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

            while (true)
            {
                WindowInfo.Width = Screens.Primary.Bounds.Width;
                WindowInfo.Height = Screens.Primary.Bounds.Height;
                int xPos = -Screens.All.Select(x => x.Bounds.X).Min() - 1;
                int yPos = -Screens.All.Select(x => x.Bounds.Y).Min() - 1;
                var win = Screens.Primary.Bounds;
                Position = new PixelPoint(xPos, yPos);
                Width = win.Width - 2;
                Height = win.Height - 2;
                await Task.Delay(10000);
            }
        }

        private async void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            TilesPanel? tp = this.FindControl<ItemsControl>("tiles").ItemsPanel.Build() as TilesPanel;
            tp?.Drag(sender as Button, 100, 100);

            var path = ((sender as Button)?.CommandParameter as TileItem)?.Path ?? ((sender as Button)?.CommandParameter as TileFolder)?.Path;
            if (Directory.Exists(path))
            {
                tiles.Clear();
                tiles.AddRange(TileUtil.GetTilesFormFolder(path));

                this.FindControl<Button>("BackButton").IsVisible = true;
            }
            else
            {
                Process proc = new Process();
                proc.EnableRaisingEvents = false;
                proc.StartInfo.UseShellExecute = true;
                proc.StartInfo.FileName = path;
                proc.Start();
            }

            (sender as Button)?.Classes.Add("openAnimation");
            await Task.Delay(5000);
            (sender as Button)?.Classes.Remove("openAnimation");
        }
        private async void AddTiles()
        {
            tiles.Clear();
            tiles.AddRange(TileUtil.GetTilesFormFolder(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)));

        }

        private async void ShowDesktop(object? sender, RoutedEventArgs e)
        {
            this.FindControl<ContextMenu>("Menu").Close();

            tiles.Clear();
            tiles.AddRange(TileUtil.GetTilesFormFolder(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)));
        }

        private async void ShowStartup(object? sender, RoutedEventArgs e)
        {
            this.FindControl<ContextMenu>("Menu").Close();

            tiles.Clear();
            tiles.AddRange(TileUtil.GetTilesFormFolder(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.StartMenu),
                    "Programs"
                )
            ));

        }

        private void Exit(object? sender, RoutedEventArgs e)
        {
            this.FindControl<ContextMenu>("Menu").Close();
            Close();
        }


        private string presetPath = "";
        private async void SetPreset(object? sender, RoutedEventArgs e)
        {
            //new SetPresetWindow().ShowDialog(this);
            presetPath = await (new OpenFolderDialog()).ShowAsync(this);
            ShowPreset(null, null);
        }

        private void ShowPreset(object? sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(presetPath))
                return;
            this.FindControl<ContextMenu>("Menu").Close();

            tiles.Clear();
            tiles.AddRange(TileUtil.GetTilesFormFolder(presetPath));
        }

        private void BackgroundClick(object? sender, PointerReleasedEventArgs e)
        {
            this.FindControl<ContextMenu>("Menu").Close();
        }

        private void OnBackButtonClick(object? sender, RoutedEventArgs e)
        {
            (sender as Button).IsVisible = false;
            ShowDesktop(null, null);
        }
    }
}
