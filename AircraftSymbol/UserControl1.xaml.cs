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

namespace AircraftSymbol
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private double LastSpeedIndicated = -1154; //for starting below 30
        private bool AnimationRunning = false;
        private int minSpeed = 20;
        public UserControl1()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();

            Canvas.SetTop(Airspeed_3, -192);
        }
        private void OnTick(object sender, EventArgs e)
        {
            AirspeedIndicated.Content = SimVars.ID.airspeedIndicated;
            TrueAirSpeed.Text = "TAS " + (int)SimVars.ID.airspeedTrue + "KT";
            if (AnimationRunning == false && SimVars.ID.airspeedIndicated >=0 && SimVars.ID.airspeedIndicated >= minSpeed)
            {
                //Idicators
                double currentAirSpeed = (((SimVars.ID.airspeedIndicated-20)/10)*56 + (-1154)) ;//52 pixels per 10mph

                AnimationRunning = true;
                DoubleAnimation doubleAnimation = new DoubleAnimation();
                doubleAnimation.From = LastSpeedIndicated;
                doubleAnimation.To = currentAirSpeed;
                //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
                
                Storyboard.SetTargetName(doubleAnimation, "Airspeed_3");
                Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Canvas.TopProperty));
                Storyboard myWidthAnimatedButtonStoryboard = new Storyboard();
                myWidthAnimatedButtonStoryboard.Children.Add(doubleAnimation);
                myWidthAnimatedButtonStoryboard.Completed += MyWidthAnimatedButtonStoryboard_Completed;
                myWidthAnimatedButtonStoryboard.Begin(Airspeed_3);

                //Speed
                DoubleAnimation SpeedAnimation = new DoubleAnimation();
                SpeedAnimation.From = LastSpeedIndicated;
                SpeedAnimation.To = currentAirSpeed;
                //doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
                SpeedAnimation.Duration = new Duration(TimeSpan.FromSeconds(.2));
                Storyboard.SetTargetName(SpeedAnimation, "Airspeed_2");
                Storyboard.SetTargetProperty(SpeedAnimation, new PropertyPath(Canvas.TopProperty));
                Storyboard SpeedStoryboard = new Storyboard();
                SpeedStoryboard.Children.Add(SpeedAnimation);
                SpeedStoryboard.Begin(Airspeed_2);

                LastSpeedIndicated = currentAirSpeed;
            }
            else
            {
                Canvas.SetTop(Airspeed_3, -1154);
            }

            //recSpeed.BeginAnimation(Canvas.TopProperty, doubleAnimation);

            /*RollScale.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            RollScale.RenderTransform = rotateTransform;*/
        }

        private void MyWidthAnimatedButtonStoryboard_Completed(object sender, EventArgs e)
        {
            AnimationRunning = false;
        }
    }
}
