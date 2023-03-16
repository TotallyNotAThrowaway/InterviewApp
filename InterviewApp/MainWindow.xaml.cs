using InterviewApp.DataModels;
using InterviewApp.Helpers;
using InterviewApp.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterviewApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Color> ColorStructToList() 
        {
            List<Color> allColors = new List<Color>();

            foreach (PropertyInfo property in typeof(Color).GetProperties()) {
                if (property.PropertyType == typeof(Color)) {
                    allColors.Add((Color) property.GetValue(null));
                }
            }

            return allColors;
        }

        public static List<Color> Colors = ColorStructToList();
        public static List<string> ColorNames = new List<string>() 
                                                {
                                                    "Crimson",
                                                    "MediumSeaGreen",
                                                    "CornflowerBlue",
                                                    "Peru",
                                                    "Coral",
                                                    "SlateGray",
                                                    "Firebrick",
                                                    "Sienna",
                                                    "Teal",
                                                    "Orange",
                                                    "OrangeRed",
                                                };

    public MainWindow()
        {
            InitializeComponent();
            pathSelectEnd.ItemsSource = MapRepository.Instance.Segments.Select(s => $"Segment №{s.Id}");
            pathSelectEnd.SelectedIndex = 29;
            pathSelectStart.ItemsSource = MapRepository.Instance.Segments.Select(s => $"Segment №{s.Id}");
            pathSelectStart.SelectedIndex = 14;
            ColorSelect.ItemsSource = ColorNames;
            StationSelect.ItemsSource = MapRepository.Instance.Stations.Select(s => s.Name);
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

        private void ColorSelect_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var color = (Color) ColorConverter.ConvertFromString((string)ColorSelect.SelectedValue);
            var station = MapRepository.Instance.Stations.FirstOrDefault(s => s.Name == (string)StationSelect.SelectedValue);

            if (station != null) {
                station.Color = color;
            }

            RailwayMap.InvalidateVisual();
            RailwayMap.UpdateLayout();
        }
    }
}
