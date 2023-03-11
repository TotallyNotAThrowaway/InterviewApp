using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.Models
{
    internal class Junction
    {
        public readonly List<Railway> Railways = new();
        public readonly Position Position;
        public Junction(Position position)
        {
            Position = position;
        }

        public Junction(int x, int y) {
            var pos = new Position() { X = x, Y = y };
            Position = pos;
        }
    }
}
