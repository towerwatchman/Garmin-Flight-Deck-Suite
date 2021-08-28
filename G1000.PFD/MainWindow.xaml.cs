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
        private DispatcherTimer timer = new DispatcherTimer();

        public MSFS.Connect.API MSFSapi = new MSFS.Connect.API();
        //Controls
        private SplashScreen.UserControl1 splashScreen = new SplashScreen.UserControl1();
        private Attitude_Pitch_Roll.UserControl1 attitude_pr = new Attitude_Pitch_Roll.UserControl1();
        private NAV.UserControl1 nav = new NAV.UserControl1();
        private SoftKeys.UserControl1 softKeys = new SoftKeys.UserControl1();
        private AircraftSymbol.UserControl1 aircraftSymbol = new AircraftSymbol.UserControl1();
        private RollScale.UserControl1 rollScale = new RollScale.UserControl1();

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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();

            MSFSapi.Init(GetHWinSource());           
        }

        private void OnTick(object sender, EventArgs e)
        {

            splashScreen.Visibility = Visibility.Hidden;
            timer.Stop();

            //Build PFD Screen
          

            /*#region Setup Layout
            attitude_pr.Width = 1024;
            attitude_pr.Height = 768;
            MainCanvas.Children.Add(attitude_pr);
            double left = (69.3);
            Canvas.SetLeft(attitude_pr, left);
            double top = (0);
            Canvas.SetTop(attitude_pr, top);
            attitude_pr.Visibility = Visibility.Visible;
            #endregion

            #region NAV
            nav.Width = 1024;
            nav.Height = 200;
            MainCanvas.Children.Add(nav);
            Canvas.SetLeft(nav, 0);
            Canvas.SetTop(nav, 0);
            nav.Visibility = Visibility.Visible;
            #endregion

            #region SoftKeys
            softKeys.Width = 1024;
            softKeys.Height = 40;
            MainCanvas.Children.Add(softKeys);
            Canvas.SetLeft(softKeys, 0);
            Canvas.SetTop(softKeys, MainCanvas.Height - 40);
            softKeys.Visibility = Visibility.Visible;
            #endregion

            #region Roll Scale
            rollScale.Width = 417.86;
            rollScale.Height = 417.86;
            MainCanvas.Children.Add(rollScale);
            Canvas.SetLeft(rollScale, 268);
            Canvas.SetTop(rollScale, 60);
            rollScale.Visibility = Visibility.Visible;
            #endregion

            #region Aircraft Symbol
            aircraftSymbol.Width = 1024;
            aircraftSymbol.Height = 768;
            MainCanvas.Children.Add(aircraftSymbol);
            Canvas.SetLeft(aircraftSymbol, 0);
            Canvas.SetTop(aircraftSymbol, 0);
            nav.Visibility = Visibility.Visible;
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
