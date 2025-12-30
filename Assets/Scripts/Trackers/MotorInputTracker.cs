using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace SensorimotorContingencies
{
    public class MotorInputTracker : Tracker
    {
        [SerializeField] 
        MotorController ToTrack;
        
        public override string MeasurementDescriptor => "transform";

        public override IEnumerable<string> CustomHeader => new string[]
            {
                "pos_x", 
                "pos_y", 
                "pos_z",
                "rot_x",
                "rot_y",
                "rot_z"
            };

        protected override UXFDataRow GetCurrentValues()
        {
            // Relative position
            Vector3 p = ToTrack.GetPosition(true);
            Quaternion qr = ToTrack.transform.rotation;
            Vector3 r = qr.eulerAngles;

            var values = new UXFDataRow()
            {
                ("pos_x", p.x),
                ("pos_y", p.y),
                ("pos_z", p.z),
                ("rot_x", r.x),
                ("rot_y", r.y),
                ("rot_z", r.z)
            };
            return values;
        }
    }
}

