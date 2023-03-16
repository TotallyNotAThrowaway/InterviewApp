using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewApp.ViewModels
{
    class WorldViewModel: ViewModelBase
    {
        List<StationViewModel> Stations { get; }

        public WorldViewModel(List<StationViewModel> stations) {
            Stations = stations;
        } 
    }
}
