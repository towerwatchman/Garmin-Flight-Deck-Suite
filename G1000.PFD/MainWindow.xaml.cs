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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init()
        {
            //Show Splashscreen
            #region Splashscreen
            splashScreen.Width = 1024;
            splashScreen.Height = 768;
            MainCanvas.Children.Add(splashScreen);
            Canvas.SetLeft(splashScreen, 0);
            Canvas.SetTop(splashScreen, 0);
            splashScreen.Visibility = Visibility.Visible;
            #endregion

            //Start Timer to show splashscreen and after a specified amonut of time (Currently 3 seconds)
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            timer.Start();

            //Get the current window and pass to API. Start API call to retrieve data
            MSFSapi.Init(GetHWinSource());           
        }

        private void OnTick(object sender, EventArgs e)
        {
            //After time has expired, hide the spash screen
            splashScreen.Visibility = Visibility.Hidden;
            timer.Stop();
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
    }
}
