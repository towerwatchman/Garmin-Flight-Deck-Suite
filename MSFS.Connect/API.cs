using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;
using Microsoft.FlightSimulator.SimConnect;
using System.Runtime.InteropServices;

namespace MSFS.Connect
{
    //Used for connection 
    enum CLIENT_DATA_DEFINITION_IDs
    {
        CLIENT_DATA_1
    }
    enum CLIENT_DATA_AREA_IDs
    {
        CLIENT_DATA_AREA_1
    }

    public enum DEFINITION
    {
        Dummy,
    };
    public enum REQUEST
    {
        Dummy = 0,
        Struct1,
    };
    enum REQUEST_IDs
    {
        REQUEST_1
    };

    public enum DATA_REQUESTS
    {
        REQUEST_1,
    };

    public enum EVENTS
    {
        PITOT_TOGGLE,
        FLAPS_INC,
        FLAPS_DEC,
        FLAPS_UP,
        FLAPS_DOWN,
        KEY_G1000_PFD_MENU_BUTTON,
        ONESEC,
    };

    enum EVENT_IDs
    {
        WASM_EVENT_1
    }

    struct ClientDataStruct
    {
        public double HEADING;
        public double Dummyval;
    };

    enum EVENT_NOTIFICATION_GROUP_IDs
    {
        EVENT_NOTIFICATION_GROUP_1
    }


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
                simconnect = new SimConnect("Simconnect - Data Retriever", m_hWnd, WM_USER_SIMCONNECT, null, 0);

                //WasmModuleClient.GetLVarList(simconnect);
                /// Listen to connect and quit msgs
                simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

                //List for events
                simconnect.OnRecvEvent += new SimConnect.RecvEventEventHandler(SimConnect_OnRecvEvent);

                /// Listen to exceptions
                simconnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);

                //Subscribe to sim variables
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Title", null, SIMCONNECT_DATATYPE.STRING256, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Latitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Longitude", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Altitude", "feet", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Airspeed Indicated", "knot", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Pitch Degrees", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Plane Bank Degrees", "degrees", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);
                simconnect.AddToDataDefinition(DEFINITION.Dummy, "Airspeed True", "knot", SIMCONNECT_DATATYPE.FLOAT64, 0, SimConnect.SIMCONNECT_UNUSED);

                // subscribe to pitot heat switch toggle
                //simconnect.MapClientEventToSimEvent(EVENTS.PITOT_TOGGLE, "PITOT_HEAT_TOGGLE");
                //simconnect.AddClientEventToNotificationGroup(NOTIFICATION_GROUPS.GROUP0, EVENTS.PITOT_TOGGLE, false);

                simconnect.RegisterDataDefineStruct<Struct1>(DEFINITION.Dummy);

                /// Catch a simobject data request
                simconnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);
            }
            catch (COMException ex)
            {
                Console.WriteLine("Connection to MSFS failed: " + ex.Message);
            }
        }

        private void Simconnect_OnRecvClientData(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            // Note: data provides more information: data.requestID for example provides the Request ID of the RequestClientData call. 

            // Now we create a data struct in order to read the data
            //ClientDataStruct ReadClientDataStruct = (ClientDataStruct)data.dwData[0];

            // From this point on, we can use the names that we declared in the struct in order to access the values
            // Note that there can be multiple values. If I understand the documentation correctly, we can use 512 double 
            // values in one client data area. If we need more data we need to create multiple client data areas. 
            //Console.WriteLine("Data received: Heading: " + ReadClientDataStruct.HEADING);
            //Console.WriteLine("Data received: " + ReadClientDataStruct.Dummyval);

            // For this example we need to fire an event to the wasm module to initiate the transfer of the LVARS to the
            // client data area. 
            //simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENT_IDs.WASM_EVENT_1, 2, EVENT_NOTIFICATION_GROUP_IDs.EVENT_NOTIFICATION_GROUP_1, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
        }

        private void SimConnect_OnRecvEvent(SimConnect sender, SIMCONNECT_RECV_EVENT recEvent)
        {
            switch (recEvent.uEventID)
            {
                /*case (uint)EVENTS.KEY_G1000_PFD_SOFTKEY1:
                    SimVars.ID.KEY_G1000_PFD_SOFTKEY1 = 1;
                    break;

                case (uint)EVENTS.KEY_G1000_PFD_SOFTKEY2:
                    SimVars.ID.KEY_G1000_PFD_SOFTKEY1 = 2;
                    break;*/
            }
            SimVars.ID.IsPressed = true;
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            //Console.WriteLine("SimConnect_OnRecvOpen");
            Console.WriteLine("Connected to SimConnect");
            m_oTimer.Start();
            bOddTick = false;
            /*
            //Clent data from WASM
            Console.WriteLine("OnRecvOpen ist erreicht. Version: V" + data.dwApplicationVersionMajor + "." + data.dwApplicationVersionMinor);

            // Map a clientdata name to a client data area ID. 
            // The name MUST be the same as it's used in the WASM Module. 
            simconnect.MapClientDataNameToID("EFIS_CDA", CLIENT_DATA_AREA_IDs.CLIENT_DATA_AREA_1);

            // Then we need to add some memory space for the client data area. 
            // Note: Convert.ToUInt32(Marshal.SizeOf(typeof(ClientDataStruct))) determines the size of the struct. 
            //       In this example with two double values the size will be 16 (bytes);
            simconnect.AddToClientDataDefinition(CLIENT_DATA_DEFINITION_IDs.CLIENT_DATA_1, SimConnect.SIMCONNECT_CLIENTDATAOFFSET_AUTO, Convert.ToUInt32(Marshal.SizeOf(typeof(ClientDataStruct))), 0.0f, SimConnect.SIMCONNECT_UNUSED);

            // THIS IS THE SECRET SAUCE!!!
            // The following step is mandatory for C# implementations. We need to register the struct in order to get data that make sense. 
            // If this step is missing. There will be events with data. But none of them will be equal to the data that have been sent. 
            // simconnect.RegisterDataDefineStruct<Struct1>(CLIENT_DATA_DEFINITION_IDs.CLIENT_DATA_1);
            simconnect.RegisterDataDefineStruct<Struct1>(CLIENT_DATA_DEFINITION_IDs.CLIENT_DATA_1);

            // Then we create the client data area
            simconnect.CreateClientData(CLIENT_DATA_AREA_IDs.CLIENT_DATA_AREA_1, Convert.ToUInt32(Marshal.SizeOf(typeof(ClientDataStruct))), SIMCONNECT_CREATE_CLIENT_DATA_FLAG.DEFAULT);

            // Then we need an event, that will be called by the sim with the data. 
            simconnect.OnRecvClientData += Simconnect_OnRecvClientData1;


            // Then we initiate the Request of the client Data area.           
            simconnect.RequestClientData(CLIENT_DATA_AREA_IDs.CLIENT_DATA_AREA_1
                , REQUEST_IDs.REQUEST_1
                , CLIENT_DATA_DEFINITION_IDs.CLIENT_DATA_1
                , SIMCONNECT_CLIENT_DATA_PERIOD.SECOND          // In this case: We want to receive the data every second. 
                , SIMCONNECT_CLIENT_DATA_REQUEST_FLAG.DEFAULT   // No matter if the data has changed. 
                , 0
                , 0
                , 0);

            // In this example, we need to send an event to the wasm module. 
            // This event is used to start the transfer of the LVAR values to the client data area .
            // *** I'm not happy with that solution. I will change that: The WASM module should read the LVARs periodically and write into the 
            //     Client Data area. But for now, I leave it as it is. 
            simconnect.MapClientEventToSimEvent(EVENT_IDs.WASM_EVENT_1, "LVAR_ACCESS.EFIS");
            simconnect.AddClientEventToNotificationGroup(EVENT_NOTIFICATION_GROUP_IDs.EVENT_NOTIFICATION_GROUP_1, EVENT_IDs.WASM_EVENT_1, true);
            simconnect.SetNotificationGroupPriority(EVENT_NOTIFICATION_GROUP_IDs.EVENT_NOTIFICATION_GROUP_1, SimConnect.SIMCONNECT_GROUP_PRIORITY_HIGHEST);
            */
        }

        private void Simconnect_OnRecvClientData1(SimConnect sender, SIMCONNECT_RECV_CLIENT_DATA data)
        {
            // Note: data provides more information: data.requestID for example provides the Request ID of the RequestClientData call. 

            // Now we create a data struct in order to read the data
            try
            {
                ClientDataStruct ReadClientDataStruct = (ClientDataStruct)data.dwData[0];

                // From this point on, we can use the names that we declared in the struct in order to access the values
                // Note that there can be multiple values. If I understand the documentation correctly, we can use 512 double 
                // values in one client data area. If we need more data we need to create multiple client data areas. 
                Console.WriteLine("Data received: Heading: " + ReadClientDataStruct.HEADING);
                Console.WriteLine("Data received: ■■■■■: " + ReadClientDataStruct.Dummyval);

                // For this example we need to fire an event to the wasm module to initiate the transfer of the LVARS to the
                // client data area. 
                simconnect.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, EVENT_IDs.WASM_EVENT_1, 2, EVENT_NOTIFICATION_GROUP_IDs.EVENT_NOTIFICATION_GROUP_1, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
            }
            catch(Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
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
            Console.WriteLine("Exception: " + eException.ToString());

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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
        }
        private void OnTick(object sender, EventArgs e)
        {
            bOddTick = !bOddTick;
            simconnect.RequestDataOnSimObjectType(DATA_REQUESTS.REQUEST_1, DEFINITION.Dummy, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
        }


    }
}
