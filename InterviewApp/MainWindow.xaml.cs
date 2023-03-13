using InterviewApp.Helpers;
using InterviewApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

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
            pathSelectStart.ItemsSource = MapRepository.Instance.Segments.Select(s => $"Segment №{s.Id}");
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RailwaysDrawingHelper.pathEndNode = MapRepository.Instance.Segments[pathSelectEnd.SelectedIndex];
            RailwayMap.InvalidateVisual();
            RailwayMap.UpdateLayout();
        }

        private void pathSelectStart_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RailwaysDrawingHelper.pathStartNode = MapRepository.Instance.Segments[pathSelectStart.SelectedIndex];
            RailwayMap.InvalidateVisual();
            RailwayMap.UpdateLayout();

        }
    }
}
