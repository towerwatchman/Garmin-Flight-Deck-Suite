using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSFS.Planes
{
    static public class Data
    {
        static public Plane Cesna172S()
        {
            Plane plane = new Plane();
            plane.Vr = 55;
            plane.Va = "90 - 105";
            plane.Vno = 129;
            plane.Vy = 74;
            plane.Vx = 62;
            plane.Vne = 163;
            plane.Vso = 40;
            plane.Vsl = 48;

            return plane;
        }
    }
}
