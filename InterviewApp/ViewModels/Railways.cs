using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using InterviewApp.Helpers;

namespace InterviewApp.ViewModels
{
    class Railways
    {
        private static Pen RailwayPen = new Pen(Brushes.Black, 2);
        private static Pen JunctionPen = new Pen(Brushes.Red, 3);
        //private static Brush StationBrush = new 

        public static void Draw(DrawingContext context) {
            foreach (var segment in MapRepository.Instance.Segments) {
                context.DrawLine(RailwayPen, segment.Start, segment.End);
            }
            foreach (var junction in MapRepository.Instance.Junctions) {
                context.DrawEllipse(null, JunctionPen, junction.Position, 3, 3);
            }
        }
    }
}
