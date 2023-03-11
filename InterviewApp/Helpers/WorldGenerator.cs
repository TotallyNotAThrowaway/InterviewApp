using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewApp.Models;

namespace InterviewApp.Helpers
{
    internal static class WorldGenerator
    {
        public static World GenerateWorld() {
            var world = new World();
            
            world.Junctions.Add(new Junction(10, 10));
            world.Junctions.Add(new Junction(50, 10));
            world.Railays.Add(new Railway(world.Junctions[0], world.Junctions[1]));

            world.Junctions.Add(new Junction(10, 30));
            world.Junctions.Add(new Junction(50, 30));
            world.Railays.Add(new Railway(world.Junctions[2], world.Junctions[3]));

            world.Railays.Add(new Railway(world.Junctions[0], world.Junctions[3]));

            return world;
        }
    }
}
