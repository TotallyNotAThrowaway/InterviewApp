using InterviewApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterviewApp.Controls
{
    /// <summary>
    /// Interaction logic for MControl.xaml
    /// </summary>
    public partial class MControl : UserControl
    {
        public WorldViewModel WorldViewModel { get; }

        public MControl()
        {
            InitializeComponent();
            WorldViewModel = new WorldViewModel();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            RailwaysDrawingHelper.Draw(drawingContext);
        }

        static MControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MControl), new FrameworkPropertyMetadata(typeof(MControl)));
        }
    }
}
