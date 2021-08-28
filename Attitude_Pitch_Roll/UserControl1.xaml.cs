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
using SharpVectors.Converters;

namespace Attitude_Pitch_Roll
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public UserControl1()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            double pitch = 0;
            if(SimVars.ID.attitude_pitch < 0)//going up
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
            Attitude_Base.RenderTransformOrigin = new Point(0.465625, 0.358724);
            TranslateTransform translateTransform = new TranslateTransform();
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            translateTransform.Y = pitch;
            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            Attitude_Base.RenderTransform = transformGroup;





            /*
            #region Attitude
            attitude_pr.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform(SimVars.ID.attitude_roll);
            attitude_pr.RenderTransform = rotateTransform;

            Canvas.SetTop(attitude_pr, (-860 - (SimVars.ID.attitude_pitch*6.5)));
         
            //MoveTo(attitude_pr, SimVars.ID.attitude_pitch);



            #endregion   */

            /*#region Setup Layout
           attitude_pr.Width = 3072;
           attitude_pr.Height = 2304;
           MainCanvas.Children.Add(attitude_pr);
           double left = (-1024);
           Canvas.SetLeft(attitude_pr, left);

           double top = (-800);
           Canvas.SetTop(attitude_pr, top);
           attitude_pr.Visibility = Visibility.Visible;
           #endregion

           #region Setup COM
           com.Width = 1024;
           com.Height = 200;
           MainCanvas.Children.Add(com);
           Canvas.SetLeft(com, 0);
           Canvas.SetTop(com, 0);
           com.Visibility = Visibility.Visible;
           #endregion

           */
        }
    }
}
