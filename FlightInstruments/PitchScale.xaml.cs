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
    /// Interaction logic for PitchScale.xaml
    /// </summary>
    public partial class PitchScale : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public PitchScale()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();
        }
        private void OnTick(object sender, EventArgs e)
        {
            //18pixels per degree

            double pitch = 0;
            if (SimVars.ID.attitude_pitch < 0)//going up
            {
                pitch = 18 * Math.Abs(SimVars.ID.attitude_pitch);
            }
            else
            {
                pitch = -(18 * Math.Abs(SimVars.ID.attitude_pitch));
            }

            //Console.Out.WriteLine("Pitch:" + SimVars.ID.attitude_pitch);
            //Console.Out.WriteLine("Pitch: " + SimVars.ID.attitude_pitch);

            //Attitude_Base.RenderTransformOrigin = new Point(0.481, 0.42);
            Pitch.RenderTransformOrigin = new Point(0.5, 0.5);
            TranslateTransform translateTransform = new TranslateTransform();
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            translateTransform.Y = pitch;
            TransformGroup transformGroup = new TransformGroup();

            PitchScaleImg.RenderTransformOrigin = new Point(0.5, 0.5);
            translateTransform = new TranslateTransform();
            rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            translateTransform.Y = pitch;
            TransformGroup transformGroup1 = new TransformGroup();
            transformGroup1.Children.Add(translateTransform);
            transformGroup1.Children.Add(rotateTransform);


            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            Pitch.RenderTransform = transformGroup;

            PitchScaleImg.RenderTransform = transformGroup1;

        }
    }
}
