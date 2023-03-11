using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace InterviewApp.Models
{
    internal class Station
    {
        private const int drawingOffset = 10;
        private SolidColorBrush brush;

        public List<Railway> Railways = new();

        public Station(Color color) 
        {
            brush = new SolidColorBrush(color);
        }

        public Rect CalculateRect() {
            var allPoints = Railways.SelectMany(r => r.Points);
            var minX = allPoints.Min(p => p.X);
            var maxX = allPoints.Max(p => p.X);
            var minY = allPoints.Min(p => p.Y);
            var maxY = allPoints.Max(p => p.Y);
            var width = maxX - minX + drawingOffset * 2;
            var height = maxY - minY + drawingOffset * 2;

            return new Rect(minX - drawingOffset, minY - drawingOffset, width, height);
        }

        public void Draw(DrawingContext context) {
            context.PushOpacity(0.3d);
            context.DrawRoundedRectangle(brush, null, CalculateRect(), drawingOffset, drawingOffset);
            context.Pop();
        }
    }
}
