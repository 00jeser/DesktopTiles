using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using DesktopTiles.Models;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace DesktopTiles.Utils
{
    public static class TileUtil
    {
        public static IEnumerable<Tile> GetTilesFormFolder(string path)
        {
            foreach (var p in Directory.GetDirectories(path))
            {
                yield return new TileFolder(p);
            }
            foreach (var p in Directory.GetFiles(path))
            {
                if (!p.EndsWith(".ini"))
                    yield return new TileItem(p);
            }
        }

        public static async Task<Bitmap> GetIcon(string path)
        {
            if (!File.Exists(path))
                return null;
            Avalonia.Media.Imaging.Bitmap AvIrBitmap = null;
            if (path.EndsWith(".ico"))
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await Task.Run(() =>
                    {
                        var fi = new System.Drawing.Icon(path, 512, 512);
                        fi.ToBitmap().Save(memory, ImageFormat.Png);
                        memory.Position = 0;
                        AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
                    });
                }
            }
            else
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    await Task.Run(() =>
                    {
                        var fi = System.Drawing.Icon.ExtractAssociatedIcon(path);
                        fi.ToBitmap().Save(memory, ImageFormat.Png);
                        memory.Position = 0;
                        AvIrBitmap = new Avalonia.Media.Imaging.Bitmap(memory);
                    });
                }
            }

            return AvIrBitmap;
        }
    }
}
