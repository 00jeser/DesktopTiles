using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DesktopTiles.Utils;
using ReactiveUI;

namespace DesktopTiles.Models
{
    public class TileItem : Tile
    {

        private Bitmap icon;

        public Bitmap Icon
        {
            get => icon;
            set => this.RaiseAndSetIfChanged(ref icon, value);
        }

        public TileItem(string p)
        {
            base.Name = p.Split('\\').Last();
            base.Path = p;
            if (Name.EndsWith(".lnk"))
                Name = Name.Split(".lnk")[0];
            if (Name.EndsWith(".url"))
            {
                Name = Name.Split(".url")[0];
                SetURLIcon(p);
            }
            else
                SetIcon(p);
        }

        private async void SetIcon(string p)
        {
            Icon = await TileUtil.GetIcon(p);
        }
        private async void SetURLIcon(string p)
        {
            var IconPath = File.ReadAllLines(p).FirstOrDefault(x => x.StartsWith("IconFile")).Split("=")[1];
            Icon = await TileUtil.GetIcon(IconPath);
        }
    }
}
