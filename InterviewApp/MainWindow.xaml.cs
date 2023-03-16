using InterviewApp.Helpers;
using InterviewApp.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InterviewApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pathSelectEnd.ItemsSource = MapRepository.Instance.Segments.Select(s => $"Segment №{s.Id}");
            pathSelectEnd.SelectedIndex = 29;
            pathSelectStart.ItemsSource = MapRepository.Instance.Segments.Select(s => $"Segment №{s.Id}");
            pathSelectStart.SelectedIndex = 14;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RailwaysDrawingHelper.PathEndNode = MapRepository.Instance.Segments[pathSelectEnd.SelectedIndex];
            RailwayMap.InvalidateVisual();
            RailwayMap.UpdateLayout();
        }

        private void PathSelectStart_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RailwaysDrawingHelper.PathStartNode = MapRepository.Instance.Segments[pathSelectStart.SelectedIndex];
            RailwayMap.InvalidateVisual();
            RailwayMap.UpdateLayout();

        }
    }
}
