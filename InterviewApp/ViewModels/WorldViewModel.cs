using InterviewApp.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace InterviewApp.ViewModels
{
    public class WorldViewModel : ViewModelBase {
        private List<StationViewModel> Stations { get; }

        public WorldViewModel(List<StationViewModel> stations) {
            Stations = stations;
        }

        public WorldViewModel() {
            var stations = MapRepository.Instance.Stations;
            var stationsViewModels = stations.Select(s =>
                                                     new StationViewModel(s.Name,
                                                                          ConvexHullHelper.GetPolygon(s.AllPoints),
                                                                          Color.FromRgb(255, 120, 120))).ToList();
            Stations = stationsViewModels;
        }
    }
}
