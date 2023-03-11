using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace InterviewApp.Models
{
    internal class Junction {
        private readonly Pen drawingPen = new Pen(Brushes.Brown, 2.0d);

        // not all railwys can go to all others in some junctions
        public readonly List<Railway> Railways = new();
        public readonly Position Position;
        public Junction(Position position) {
            Position = position;
        }

        public Junction(int x, int y) {
            var pos = new Position() { X = x, Y = y };
            Position = pos;
        }

        public void Draw(DrawingContext context) 
        {
            context.DrawEllipse(null, drawingPen, new Point(Position.X, Position.Y), 3, 3);
        }
    }
}
