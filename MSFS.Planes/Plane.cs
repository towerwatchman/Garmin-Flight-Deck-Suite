namespace MSFS.Planes
{
    public class Plane
    {
        public int Vso; //Stall in landing configurations
        public int Vsl; //Stall in cruise configuration
        public int Vr; //Rotation Speed
        public int Vx; //Best Rate of Climb
        public int Vy; //Best angle-of-climb
        public int Vno; //Max. structural cruising speed
        public int Vne; //Never exceed speed 
        public string Va; //Design Maneuvering
    }
    /*
     *  Rotation speed (Vr)	55 KIAS
        Short Field Takeoff, Flaps 10°, Speed at 50 feet	56 KIAS
        Design Maneuvering (Va)	90-105 KIAS
        Maximum Structural Cruising (Vno)	129 KIAS
        Normal climb	75-85 KIAS
        Best rate-of-climb (Vy)	74 KIAS
        (10,000ft: 72 KIAS)
        Best angle-of-climb (Vx)	62 KIAS
        (10,000ft: 67 KIAS)
        Cruise speed	124 KIAS
        Never Exceed (Vne)	163 KIAS
        Best Glide	68 KIAS
        Final Approach, Normal Landing, flaps 0°	65-75 KIAS
        Final Approach, Normal Landing, flaps 40°	60-70 KIAS
        Final Approach, Short-Field Landing, flaps 40°	61 KIAS
        Final Approach, Soft-Field Landing, flaps 40°	70? MPH
        Max Flaps Extended Speed, flaps 10° (Vfe)	110 KIAS
        Max Flaps Extended Speed, flaps 10-30° (Vfe)	85 KIAS
     * */
}