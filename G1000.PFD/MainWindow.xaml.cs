using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace G1000.PFD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //------
        //Vars for transforms
        //double lastAttitudePitch = 0;
        //double lastAttitudeRoll = 0;

        //------

        private DispatcherTimer timer = new DispatcherTimer();

        public MSFS.Connect.API MSFSapi = new MSFS.Connect.API();
        //Controls
        private SplashScreen.UserControl1 splashScreen = new SplashScreen.UserControl1();
        private Attitude_Pitch_Roll.UserControl1 attitude_pr = new Attitude_Pitch_Roll.UserControl1();
        private COM.UserControl1 com = new COM.UserControl1();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init()
        {
            //Show Splashscreen for 3 seconds
            #region Splashscreen
            splashScreen.Width = 1024;
            splashScreen.Height = 768;
            MainCanvas.Children.Add(splashScreen);
            Canvas.SetLeft(splashScreen, 0);
            Canvas.SetTop(splashScreen, 0);
            splashScreen.Visibility = Visibility.Visible;
            #endregion

            //Start Timer to show splashscreen and after a specified amonut of time
            timer.Interval = new TimeSpan(0, 0, 0, 0, 3000);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();

            MSFSapi.Init(GetHWinSource());           
        }

        private void OnTick(object sender, EventArgs e)
        {
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

        protected HwndSource GetHWinSource()
        {
            return PresentationSource.FromVisual(this) as HwndSource;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MSFSapi.SetWindowHandle(GetHWinSource().Handle);
            Init();
        }

        public void MoveTo(UserControl target, double newX)
        {
            try
            {
                TranslateTransform trans = new TranslateTransform();
                target.RenderTransform = trans;
                DoubleAnimation anim2 = new DoubleAnimation(0, 5, TimeSpan.FromSeconds(.2));
                trans.BeginAnimation(TranslateTransform.XProperty, anim2);

                /*Vector offset = VisualTreeHelper.GetOffset(target);
                var left = offset.X;
                TranslateTransform trans = new TranslateTransform();
                target.RenderTransform = trans;
                DoubleAnimation anim2 = new DoubleAnimation(0, newX - left, TimeSpan.FromSeconds(.2));
                trans.BeginAnimation(TranslateTransform.XProperty, anim2);*/
            }
            catch(Exception ex)
            {

            }
        }
    }
}
