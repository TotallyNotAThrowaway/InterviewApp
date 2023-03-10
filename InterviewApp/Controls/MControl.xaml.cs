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
    /// Interaction logic for MControl.xaml
    /// </summary>
    public partial class MControl : UserControl
    {
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
            drawingContext.DrawEllipse(Brushes.Blue, null, new Point(50, 50), 50, 50);
        }

        static MControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MControl), new FrameworkPropertyMetadata(typeof(MControl)));
        }
    }
}
