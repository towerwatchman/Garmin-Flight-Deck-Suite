using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimVars
{
    public static class ID
    {
        static public String titlesimconnect { get; set; }
        static public double latitude{get; set;}
        static public double longitude{get; set;}
        static public double altitude{get; set;}
        static public double airspeedIndicated{get; set;}
        static public double attitude_roll{get; set;}
        static public double attitude_pitch{get; set;}
        static public double airspeedTrue { get; set; }
    }
}
