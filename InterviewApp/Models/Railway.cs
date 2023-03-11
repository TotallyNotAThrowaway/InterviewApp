using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace InterviewApp.Models
{
    internal class Railway
    {
        private static readonly Pen drawingPen = new Pen(Brushes.Black, 1.0d);

        private Junction start;
        private Junction end;

        public Position[] Points => new[] { start.Position, end.Position };

        public readonly double Length;

        public Railway(Junction Start, Junction End)
        {
            start = Start;
            end = End;
            var xDiffSquared = Math.Pow(Start.Position.X - End.Position.X, 2);
            var yDiffSquared = Math.Pow(Start.Position.Y - End.Position.Y, 2);
            Length = Math.Sqrt(xDiffSquared + yDiffSquared);
            Start.Railways.Add(this);
            End.Railways.Add(this);
        }

        public void Draw(DrawingContext context) 
        {
            context.DrawLine(drawingPen, new Point(start.Position.X, start.Position.Y),
                                         new Point(end.Position.X, end.Position.Y));
        }
    }
}
