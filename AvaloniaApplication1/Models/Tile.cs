using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvaloniaApplication1.Models
{
    public class Tile : ReactiveObject
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string Path { get; set; }
    }
}
