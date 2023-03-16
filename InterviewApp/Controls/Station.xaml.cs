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

namespace InterviewApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для Station.xaml
    /// </summary>
    public partial class Station : UserControl
    {
        private StationViewModel viewModel;

        public Station() {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            viewModel.Draw(drawingContext);
        }
    }
}
