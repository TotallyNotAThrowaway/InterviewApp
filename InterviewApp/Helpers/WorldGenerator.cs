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
        private static Random rand = new();

        public static World GenerateWorld() {
            var world = new World();

            var station = new Station(Color.FromRgb(120, 255, 120));
            world.Stations.Add(station);

            var entry = new Junction(10, 70);
            var exit1 = new Junction(300, 60);
            var exit2 = new Junction(300, 80);

            world.Junctions.Add(entry);
            world.Junctions.Add(exit1);
            world.Junctions.Add(exit2);

            for (int i = 0; i < 10; i++) {
                var randomOffset = rand.Next(50) - 25;
                var jLeft = new Junction(randomOffset + 50 + i * 10, 10 + i * 10);
                var jRight = new Junction(randomOffset + 100 + i * 10, 10 + i * 10);
                world.Junctions.Add(jLeft);
                world.Junctions.Add(jRight);
                var rail = new Railway(jLeft, jRight);
                var entryRail = new Railway(entry, jLeft);
                world.Railways.Add(rail);
                world.Railways.Add(entryRail);

                if (i < 5) {
                    world.Railways.Add(new Railway(jRight, exit1));
                }
                else {
                    world.Railways.Add(new Railway(jRight, exit2));
                }

                station.Railways.Add(rail);
            }

            var entry1 = new Junction(350, 60);
            var entry2 = new Junction(350, 80);

            world.Junctions.Add(entry1);
            world.Junctions.Add(entry2);

            var conn = new Railway(exit1, entry1);
            world.Railways.Add(conn);
            conn = new Railway(exit2, entry2);
            world.Railways.Add(conn);

            Junction last = null;
            var station2 = new Station(Color.FromRgb(120, 120, 255));
            world.Stations.Add(station2);

            for (int i = 0; i < 6; i++) {
                var randomOffset = rand.Next(50) - 25;
                var jLeft = new Junction(randomOffset + 400, 20 + i * 15);
                var jMid = new Junction(randomOffset + 400 + i * 10, 20 + i * 15);
                var jRight = new Junction(randomOffset + 500, 20 + i * 15);
                var railLeft = new Railway(jLeft, jMid);
                var railRight = new Railway(jMid, jRight);
                world.Junctions.Add(jLeft);
                world.Junctions.Add(jMid);
                world.Junctions.Add(jRight);
                world.Railways.Add(railLeft);
                station2.Railways.Add(railLeft);
                world.Railways.Add(railRight);
                station2.Railways.Add(railRight);

                Railway middleway;
                if (i < 3) {
                    middleway = new Railway(entry1, jLeft);
                }
                else {
                    middleway = new Railway(entry2, jLeft);
                }
                world.Railways.Add(middleway);


                if (last != null) {
                    var connection = new Railway(last, jMid);
                    world.Railways.Add(connection);
                    station2.Railways.Add(connection);
                }
                last = jMid;
            }

            return world;
        }
    }
}
