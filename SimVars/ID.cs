using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimVars
{
    public static class ID
    {
        private static bool _buttonPress;

        public static bool IsPressed
        {
            get { return _buttonPress; }
            set
            {
                _buttonPress = value;
                OnPressedChanged(_buttonPress);
            }
        }

        public static event EventHandler ButtonStatusChanged;
        private static void OnPressedChanged(bool boolValue)
        {
            if (ButtonStatusChanged != null)
                ButtonStatusChanged(boolValue, EventArgs.Empty);
        }
        static public String titlesimconnect { get; set; }
        static public double latitude{get; set;}
        static public double longitude{get; set;}
        static public double altitude{get; set;}
        static public double airspeedIndicated{get; set;}
        static public double attitude_roll{get; set;}
        static public double attitude_pitch{get; set;}
        static public double airspeedTrue { get; set; }

        #region PFD KEYS
        static public int KEY_G1000_PFD_SOFTKEY1 { get; set;}
        static public int KEY_G1000_PFD_SOFTKEY2 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY3 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY4 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY5 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY6 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY7 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY8 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY9 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY10 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY11 { get; set; }
        static public int KEY_G1000_PFD_SOFTKEY12 { get; set; }
        #endregion



    }
}
