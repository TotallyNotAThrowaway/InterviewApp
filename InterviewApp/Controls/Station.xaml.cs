using InterviewApp.ViewModels;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterviewApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для Station.xaml
    /// </summary>
    public partial class Station : UserControl
    {
        private readonly StationViewModel viewModel;

        public Station() {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            viewModel.Draw(drawingContext);
        }
    }
}
