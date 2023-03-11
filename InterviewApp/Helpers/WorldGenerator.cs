using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterviewApp.Models;
using System.Windows.Media;

namespace InterviewApp.Helpers
{
    internal static class WorldGenerator
    {
        public static World GenerateWorld() {
            var world = new World();

            var station = new Station(Color.FromRgb(120, 255, 120));
            world.Stations.Add(station);

            var entry = new Junction(10, 70);
            var exit1 = new Junction(200, 50);
            var exit2 = new Junction(200, 90);

            for (int i = 0; i < 10; i++) {
                var jLeft = new Junction(50 + i * 10, 10 + i * 10);
                var jRight = new Junction(100 + i * 10, 10 + i * 10);
                world.Junctions.Add(jLeft);
                world.Junctions.Add(jRight);
                var rail = new Railway(jLeft, jRight);
                var entryRail = new Railway(entry, jLeft);
                world.Railways.Add(rail);
                world.Railways.Add(entryRail);

                if (i < 5) {
                    world.Railways.Add
                }

                station.Railways.Add(rail);
            }

            //world.Junctions.Add(new Junction(10, 10));
            //world.Junctions.Add(new Junction(50, 10));
            //world.Railays.Add(new Railway(world.Junctions[0], world.Junctions[1]));

            //world.Junctions.Add(new Junction(10, 30));
            //world.Junctions.Add(new Junction(50, 30));
            //world.Railays.Add(new Railway(world.Junctions[2], world.Junctions[3]));

            //world.Railays.Add(new Railway(world.Junctions[0], world.Junctions[3]));

            return world;
        }
    }
}
