using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;
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

        public static async Task<Color> GetMainColor(string path)
        {
            if (!File.Exists(path))
                return Colors.Transparent;
            Dictionary<int, int> colorsCounts = new();

            int SatrationCount = 0, BrightnessCount = 0;
            float SatrationSumm = 0, BrightnessSumm = 0;

            await Task.Run(() =>
                {
                    var fi = System.Drawing.Icon.ExtractAssociatedIcon(path).ToBitmap();
                    for (int i = 0; i < fi.Width; i++)
                    {
                        for (int j = 0; j < fi.Height; j++)
                        {
                            if(i <= fi.Width / 5 * 2 && j >= fi.Height / 5 * 2)
                                continue;
                            var c = fi.GetPixel(i, j);
                            //c = System.Drawing.Color.FromArgb((byte)(c.R & 0b11111110), (byte)(c.G & 0b11111110), (byte)(c.B & 0b11111110));
                            if (c.A == byte.MaxValue)
                            {
                                int cH = (int)c.GetHue();
                                SatrationCount++;
                                BrightnessCount++;
                                SatrationSumm += c.GetSaturation();
                                BrightnessSumm += c.GetBrightness();
                                if (colorsCounts.ContainsKey(cH))
                                {
                                    colorsCounts[cH]++;
                                }
                                else
                                {
                                    colorsCounts[cH] = 1;
                                }
                            }
                        }
                    }
                });
            if (colorsCounts.Count > 0)
            {
                var maxHue = colorsCounts.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                return ColorFromHSV(maxHue, SatrationSumm / SatrationCount, BrightnessSumm / BrightnessCount);
            }

            return Colors.White;
        }
        private static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, (byte)v, (byte)t, (byte)p);
            else if (hi == 1)
                return Color.FromArgb(255, (byte)q, (byte)v, (byte)p);
            else if (hi == 2)
                return Color.FromArgb(255, (byte)p, (byte)v, (byte)t);
            else if (hi == 3)
                return Color.FromArgb(255, (byte)p, (byte)q, (byte)v);
            else if (hi == 4)
                return Color.FromArgb(255, (byte)t, (byte)p, (byte)v);
            else
                return Color.FromArgb(255, (byte)v, (byte)p, (byte)q);
        }
    }
}
