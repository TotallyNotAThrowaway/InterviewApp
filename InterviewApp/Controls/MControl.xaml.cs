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
using InterviewApp.Models;
using InterviewApp.Helpers;
using InterviewApp.ViewModels;

namespace InterviewApp.Controls
{
    /// <summary>
    /// Interaction logic for MControl.xaml
    /// </summary>
    public partial class MControl : UserControl
    {
        private readonly World world = WorldGenerator.GenerateWorld();

        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(string), typeof(MControl), new PropertyMetadata(null));

        public string MyProperty
        {
            get { return (string)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public MControl()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            //drawingContext.DrawEllipse(Brushes.Blue, null, new Point(50, 50), 50, 50);

            /* this is the old solution
            foreach (var railway in world.Railways) {
                railway.Draw(drawingContext);
            }

            foreach (var junction in world.Junctions) {
                junction.Draw(drawingContext);
            }

            foreach (var station in world.Stations) {
                //station.Draw(drawingContext);
                station.Draw2(drawingContext);
            }
            */

            Railways.Draw(drawingContext);
        }

        static MControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MControl), new FrameworkPropertyMetadata(typeof(MControl)));
        }
    }
}
