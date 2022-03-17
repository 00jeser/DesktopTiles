using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using AvaloniaApplication1.Models;

namespace AvaloniaApplication1.Controls
{
    public class TilesPanel : Panel
    {
        public static readonly DirectProperty<TilesPanel, double> SpacingProperty =
            AvaloniaProperty.RegisterDirect<TilesPanel, double>(
                nameof(Spacing),
                o => o.Spacing,
                (o, v) => o.Spacing = v);

        private double _spacing;

        public double Spacing
        {
            get => _spacing;
            set => SetAndRaise(SpacingProperty, ref _spacing, value);
        }
        public static readonly DirectProperty<TilesPanel, double> ElementSizeProperty =
            AvaloniaProperty.RegisterDirect<TilesPanel, double>(
                nameof(ElementSize),
                o => o.ElementSize,
                (o, v) => o.ElementSize = v);

        private double _elementSize;

        public double ElementSize
        {
            get => _elementSize;
            set => SetAndRaise(ElementSizeProperty, ref _elementSize, value);
        }

        private int rows, columns;


        private IControl draggingTile;
        private double draggingx, draggingy;
        public void Drag(IControl tile, double x, double y)
        {
            draggingTile = tile;
            draggingx = x; draggingy = y;
            Draw();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            CalculateRowsAndColumns();
            Draw();
            var panelDesiredSize = GetPanelSize;

            return panelDesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            CalculateRowsAndColumns();
            Draw();
            var s = GetPanelSize;
            return s;
        }

        private void Draw()
        {
            int row = 0;
            int col = 0;
            foreach (var child in Children)
            {
                double x = draggingx;
                double y = draggingy;

                child.Arrange(new Rect(col * ElementSize + Spacing, row * ElementSize + Spacing, ElementSize - Spacing, ElementSize - Spacing));

                col++;
                if (col >= columns)
                {
                    col = 0;
                    row++;
                }
            }
        }

        private void CalculateRowsAndColumns()
        {
            rows = (int)(WindowInfo.Height / (ElementSize));
            columns = (int)(WindowInfo.Width / (ElementSize));
            if (rows < 1) rows = 1;
            if (columns < 1) columns = 1;
        }
        private Size GetPanelSize => new Size(
            columns * (ElementSize) + Spacing,
            rows * (ElementSize) + Spacing
        );
    }
}
