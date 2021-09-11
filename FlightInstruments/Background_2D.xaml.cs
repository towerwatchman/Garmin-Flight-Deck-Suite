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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FlightInstruments
{
    /// <summary>
    /// Interaction logic for Background_2D.xaml
    /// </summary>
    public partial class Background_2D : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public Background_2D()
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
            AttitudeImg.RenderTransformOrigin = new Point(0.5, 0.5);
            TranslateTransform translateTransform = new TranslateTransform();
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            translateTransform.Y = pitch;
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            AttitudeImg.RenderTransform = transformGroup;
            
        }
        private void NewStoryboard(FrameworkElement Element, double From, double To, string TargetName, double Duration, PropertyPath propertyPath)
        {
            DoubleAnimation Animation = new DoubleAnimation();
            Animation.From = From;
            Animation.To = To;
            Animation.Duration = new Duration(TimeSpan.FromSeconds(Duration));
            Storyboard.SetTargetName(Animation, TargetName);
            Storyboard.SetTargetProperty(Animation, propertyPath);
            Storyboard StoryboardAnimation = new Storyboard();
            StoryboardAnimation.Children.Add(Animation);
            StoryboardAnimation.Begin(Element);
        }
    }
}
