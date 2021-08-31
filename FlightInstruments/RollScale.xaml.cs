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

namespace FlightInstruments
{
    /// <summary>
    /// Interaction logic for RollScale.xaml
    /// </summary>
    public partial class RollScale : UserControl
    {

        private DispatcherTimer timer = new DispatcherTimer();
        public RollScale()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();
        }
        private void OnTick(object sender, EventArgs e)
        {
            RollScaleImg.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            RollScaleImg.RenderTransform = rotateTransform;
        }
    }
}
