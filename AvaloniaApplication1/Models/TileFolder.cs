using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvaloniaApplication1.Utils;
using ReactiveUI;

namespace AvaloniaApplication1.Models
{
    public class TileFolder : Tile
    {
        public List<TileItem> Tiles { get; set; }

        private Bitmap icon1;
        public Bitmap Icon1
        {
            get => icon1;
            set => this.RaiseAndSetIfChanged(ref icon1, value);
        }
        private Bitmap icon2;
        public Bitmap Icon2
        {
            get => icon2;
            set => this.RaiseAndSetIfChanged(ref icon2, value);
        }
        private Bitmap icon3;
        public Bitmap Icon3
        {
            get => icon3;
            set => this.RaiseAndSetIfChanged(ref icon3, value);
        }
        private Bitmap icon4;
        public Bitmap Icon4
        {
            get => icon4;
            set => this.RaiseAndSetIfChanged(ref icon4, value);
        }

        public TileFolder(string p)
        {
            Tiles = new List<TileItem>(4);
            base.Path = p;
            base.Name = p.Split('\\').Last();
            SetIcons(p);
        }

        private async void SetIcons(string p)
        {
            var fs = Directory.GetFiles(p);
            if (fs.Length >= 1)
                Icon1 = await TileUtil.GetIcon(fs[0]);
            if (fs.Length >= 2)
                Icon2 = await TileUtil.GetIcon(fs[1]);
            if (fs.Length >= 3)
                Icon3 = await TileUtil.GetIcon(fs[2]);
            if (fs.Length >= 4)
                Icon4 = await TileUtil.GetIcon(fs[3]);
        }

    }
}
