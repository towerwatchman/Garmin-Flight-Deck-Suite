using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;

using Microsoft.FlightSimulator.SimConnect;

namespace MSFS.Connect
{
    public enum DEFINITION
    {
        Dummy,
    };
    public enum REQUEST
    {
        Dummy = 0,
        Struct1
    };
    enum DATA_REQUESTS
    {
        REQUEST_1,
    };

    // String properties must be packed inside of a struct
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct Struct1
    {
        // this is how you declare a fixed size string
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public String title;
        public double latitude;
        public double longitude;
        public double altitude;
        public double airspeedIndicated;
        public double attitude_pitch;
        public double attitude_roll;
        public double airspeedTrue;
    };

    public class API
    {
        private DispatcherTimer m_oTimer = new DispatcherTimer();
        //private bool bConnected = false;
        private bool bOddTick = false;

        public DEFINITION eDef = DEFINITION.Dummy;
        public REQUEST eRequest = REQUEST.Dummy;

        public const int WM_USER_SIMCONNECT = 0x0402;

        /// Window handle
        private IntPtr m_hWnd = new IntPtr(0);

        /// SimConnect object
        private SimConnect simconnect = null;
        public void SetWindowHandle(IntPtr _hWnd)
        {
            m_hWnd = _hWnd;
        }

        public void Init(HwndSource hwnd)
        {
            m_hWnd = hwnd.Handle;
            hwnd.AddHook(WndProc);
            //m_hWnd.addh
            Connect();
            m_oTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            m_oTimer.Tick += new EventHandler(OnTick);
        }

        private void Connect()
        {
            Console.WriteLine("Connect");

            try
            {
                /// The constructor is similar to SimConnect_Open in the native API
                simconnect = new SimConnect("Simconnect - Simvar test", m_hWnd, WM_USER_SIMCONNECT, null, 0);

                /// Listen to connect and quit msgs
                simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                /// Listen to exceptions
                simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Airspeed Indicated", "knot", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Pitch Degrees", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Bank Degrees", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Airspeed True", "knot", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);

                simconnect.RegisterDataDefineStruct<Struct1>(DEFINITION.Dummy);

                /// Catch a simobject data request
                simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);
            }
            catch (COMException ex)
            {
                Console.WriteLine("Connection to MSFS failed: " + ex.Message);
            }
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Console.WriteLine("SimConnect_OnRecvOpen");
            Console.WriteLine("Connected to KH");

            //bConnected = true;

            // Register pending requests
            /*foreach (string oSimvarRequest in lSimvarRequests)
            {
                if (oSimvarRequest.bPending)
                {
                    oSimvarRequest.bPending = !RegisterToSimConnect(oSimvarRequest);
                    oSimvarRequest.bStillPending = oSimvarRequest.bPending;
                }
            }*/

            m_oTimer.Start();
            bOddTick = false;
        }

        /// The case where the user closes game
        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Console.WriteLine("SimConnect_OnRecvQuit");
            Console.WriteLine("KH has exited");

            Disconnect();
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;
            Console.WriteLine("SimConnect_OnRecvException: " + eException.ToString());

        }

        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            //Console.WriteLine("SimConnect_OnRecvSimobjectDataBytype");

            uint iRequest = data.dwRequestID;
            uint iObject = data.dwObjectID;

            switch ((DATA_REQUESTS)data.dwRequestID)
            {
                case DATA_REQUESTS.REQUEST_1:
                    Struct1 s1 = (Struct1)data.dwData[0];

                    //SimVars.API.
                    SimVars.ID.titlesimconnect = s1.title;
                    SimVars.ID.latitude = s1.latitude;
                    SimVars.ID.longitude = s1.longitude;
                    SimVars.ID.altitude = s1.altitude;
                    SimVars.ID.airspeedIndicated = s1.airspeedIndicated;
                    SimVars.ID.attitude_roll = s1.attitude_roll;
                    SimVars.ID.attitude_pitch = s1.attitude_pitch;
                    SimVars.ID.airspeedTrue = s1.airspeedTrue;

                    /*Console.WriteLine("title: " + s1.title);
                    Console.WriteLine("Lat:   " + s1.latitude);
                    Console.WriteLine("Lon:   " + s1.longitude);
                    Console.WriteLine("Alt:   " + s1.altitude);
                    Console.WriteLine("Speed:   " + s1.airspeedIndicated);
                    Console.WriteLine("Roll:   " + s1.attitude_roll);
                    Console.WriteLine("Pitch:   " + s1.attitude_pitch);*/
                    //Console.Clear();
                    break;

                default:
                    Console.WriteLine("Unknown request ID: " + data.dwRequestID);
                    break;
            }
        }


        private IntPtr WndProc(IntPtr hWnd, int iMsg, IntPtr hWParam, IntPtr hLParam, ref bool bHandled)
        {
            try
            {
                if (iMsg == WM_USER_SIMCONNECT)
                {
                    simconnect?.ReceiveMessage();
                }
            }
            catch(Exception ex)
            {
                Disconnect();
            }


            return IntPtr.Zero;
        }

        public void Disconnect()
        {
            Console.WriteLine("Disconnect");

            m_oTimer.Stop();
            bOddTick = false;

            if (simconnect != null)
            {
                /// Dispose serves the same purpose as SimConnect_Close()
                simconnect.Dispose();
                simconnect = null;
            }

            //sConnectButtonLabel = "Connect";
            //bConnected = false;

            // Set all requests as pending
            /*foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                oSimvarRequest.bPending = true;
                oSimvarRequest.bStillPending = true;
            }*/
        }
        private void OnTick(object sender, EventArgs e)
        {
            //Console.WriteLine("OnTick");

            bOddTick = !bOddTick;
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_1, DEFINITION.Dummy, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
            //Console.WriteLine("Request sent...");

            /*foreach (SimvarRequest oSimvarRequest in lSimvarRequests)
            {
                if (!oSimvarRequest.bPending)
                {
                    simconnect?.RequestDataOnSimObjectType(oSimvarRequest.eRequest, oSimvarRequest.eDef, 0, m_eSimObjectType);
                    oSimvarRequest.bPending = true;
                }
                else
                {
                    oSimvarRequest.bStillPending = true;
                }
            }*/
        }


    }
}
