using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.FlightSimulator.SimConnect;
using System.Windows.Forms;
using System.Diagnostics;

namespace API_Test
{
    class Program
    {
        const int WM_USER_SIMCONNECT = 0x0402;
        enum DEFINITIONS
        {
            Struct1,
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        struct Struct1
        {
            // this is how you declare a fixed size string
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public String title;
            public double latitude;
            public double longitude;
            public double altitude;
            //public double attitude_roll;
            //public double attitude_pitch;

        };

        public static SimConnect simconnect = null;

        protected virtual void DefWndProc(ref Message m)
        {
            if (m.Msg == WM_USER_SIMCONNECT)
            {
                if (simconnect != null)
                {
                    simconnect.ReceiveMessage();
                }
            }
            else
            {
                //base.DefWndProc(ref m);
            }
        }

        static void Main(string[] args)
        {

            // Open
            // Declare a SimConnect object 
            
            // User-defined win32 event
            const int WM_USER_SIMCONNECT = 0x0402;
            try
            {

                simconnect = new SimConnect("Managed Data Request", Process.GetCurrentProcess().MainWindowHandle, WM_USER_SIMCONNECT, null, 0);
                // listen to connect and quit msgs
                simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(simconnect_OnRecvOpen);
                simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(simconnect_OnRecvQuit);

                // listen to exceptions
                simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(simconnect_OnRecvException);

                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0, SimConnect.SIMCONNECT_UNUSED);

                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);

                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);

                simconnect.AddToDataDefinition(DEFINITIONS.Struct1, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);

                simconnect.RegisterDataDefineStruct<Struct1>(DEFINITIONS.Struct1);
                Console.WriteLine("Connected");
                simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(simconnect_OnRecvSimobjectDataBytype);
            }
            catch (COMException ex)
            {
                Console.WriteLine(ex);
                // A connection to the SimConnect server could not be established 
            }
            // Close
            if (simconnect != null)
            {
                simconnect.Dispose();
                simconnect = null;
            }
            Console.Read();
        }

        private static void simconnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            Struct1 s1 = (Struct1)data.dwData[0];
            Console.Out.WriteLine(s1.latitude);
            //Console.Out.WriteLine(s1.attitude_roll);
        }
        private static void simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.Out.WriteLine("Connected to Prepar3D");
        }

        // The case where the user closes Prepar3D
        private static void simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.Out.WriteLine("Prepar3D has exited");
            //closeConnection();
        }

        private static void simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            Console.Out.WriteLine("Exception received: " + data.dwException);
        }

       
    }
}
