using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Controls
{
    public partial class Row : UserControl
    {
        public delegate void FolderChange(Row sender);
        public event FolderChange FolderOpenEvent;
        public event FolderChange FolderCloseEvent;


        public static readonly DirectProperty<Row, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<Row, IEnumerable>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);

        private IEnumerable _items = new AvaloniaList<object>();

        public IEnumerable Items
        {
            get { return _items; }
            set
            {
                SetAndRaise(ItemsProperty, ref _items, value);
                this.FindControl<ItemsControl>("rowLayout").Items = value;
            }
        }
        //private List<Tile> tiles;
        //public List<Tile> Tiles
        //{
        //    get => tiles;
        //    set
        //    {
        //        tiles = value;
        //        
        //    } }
        public Row()
        {
            InitializeComponent();
                this.FindControl<ItemsControl>("Folder").Items =
                    new ObservableCollection<TileItem>(Enumerable.Range(0, 8).Select(x => new TileItem()));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void OpenFolder()
        {
            this.FindControl<ItemsControl>("Folder").Height = 200;
            this.FindControl<ScrollViewer>("scroll").HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        }
        public void CloseFolder()
        {
                this.FindControl<ItemsControl>("Folder").Height = 0;
                this.FindControl<ScrollViewer>("scroll").HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        private bool folderOpen = false;
        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            folderOpen = !folderOpen;
            if (folderOpen)
            {
                OpenFolder();
                FolderOpenEvent?.Invoke(this);
            }
            else
            {
                CloseFolder();
                FolderCloseEvent?.Invoke(this);
            }
        }
    }
}
