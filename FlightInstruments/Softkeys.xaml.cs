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
    /// Interaction logic for Softkeys.xaml
    /// </summary>
    public partial class Softkeys : UserControl
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public Softkeys()
        {
            InitializeComponent();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += new EventHandler(OnTick);
            //timer.Start();
            Init();
        }

        private void Init()
        {
            SK1.Content = "";
            SK2.Content = "INSET";
            SK3.Content = "";
            SK4.Content = "PFD";
            SK5.Content = "OBS";
            SK6.Content = "CDI";
            SK7.Content = "DME";
            SK8.Content = "XPDR";
            SK9.Content = "INDENT";
            SK10.Content = "TMR/REF";
            SK11.Content = "NRST";
            SK12.Content = "ALERTS";

            //set up event for when a key is pressed
            SimVars.ID.ButtonStatusChanged += ID_ButtonStatusChanged;
        }

        private void ID_ButtonStatusChanged(object sender, EventArgs e)
        {
            if(SimVars.ID.KEY_G1000_PFD_SOFTKEY1 == 1)
            {
                SimVars.ID.KEY_G1000_PFD_SOFTKEY1 = 0;
                Console.Out.WriteLine("Button 1 Pressed");
            }
            if (SimVars.ID.KEY_G1000_PFD_SOFTKEY2 == 1)
            {
                SimVars.ID.KEY_G1000_PFD_SOFTKEY2 = 0;
                Console.Out.WriteLine("Button 2 Pressed");
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            
        }
    }
}
