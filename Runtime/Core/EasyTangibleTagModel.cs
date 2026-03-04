using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public class EasyTangibleTagModel
    {
        public int SessionID { get; set; }
        public int FiducialID { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        public float Angle { get; set; }
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }
        public float RotationSpeed { get; set; }
        public float MotionAccel { get; set; }
        public float RotationAccel { get; set; }
        public float Degree => Angle * Mathf.Rad2Deg; // Optional helper
    }
}
