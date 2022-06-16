using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;

namespace DesktopTiles.Views
{
    public partial class SetPresetWindow : Window
    {
        public SetPresetWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var StylesList = new List<string>{"Android", "Metro", "Win11"};
            try
            {
                StylesList.AddRange(Directory.GetFiles(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Styles"));
            }
            catch (System.Exception)
            {
                
            }
            this.FindControl<ComboBox>("StylesCB").Items = StylesList;
        }

        private void StylesCB_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
#pragma warning disable CS8601 // ��������, ����������-������, ����������� �������� NULL.
#pragma warning disable CS8600 // �������������� ��������, ������������ �������� NULL ��� ���������� �������� NULL � ���, �� ����������� �������� NULL.
#pragma warning disable CS8602 // ������������� ��������� ������ ������.
#pragma warning disable CS8604 // ��������, ��������-������, ����������� �������� NULL.
            MainWindow.Instance.Close();
            (new MainWindow(e.AddedItems[0].ToString() switch
            {
                "Metro" => (StyleInclude)MainWindow.Instance.Resources["Metro"],
                "Android" => (StyleInclude)MainWindow.Instance.Resources["Android"],
                "Win11" => (StyleInclude)MainWindow.Instance.Resources["Win11"],
                _ => AvaloniaRuntimeXamlLoader.Parse<Styles>(File.ReadAllText(e.AddedItems[0].ToString())),
            })).Show();
            File.WriteAllText("savedStyle", e.AddedItems[0].ToString());
#pragma warning restore CS8604 // ��������, ��������-������, ����������� �������� NULL.
#pragma warning restore CS8602 // ������������� ��������� ������ ������.
#pragma warning restore CS8600 // �������������� ��������, ������������ �������� NULL ��� ���������� �������� NULL � ���, �� ����������� �������� NULL.
#pragma warning restore CS8601 // ��������, ����������-������, ����������� �������� NULL.

        }
    }
}
