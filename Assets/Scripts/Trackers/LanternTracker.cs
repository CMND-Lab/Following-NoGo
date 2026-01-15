using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class LanternTracker : Tracker
    {
        [SerializeField] 
        LanternController ToTrack;
        
        public override string MeasurementDescriptor => "activation";

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
            Vector3 dist = ToTrack.GetCurrentDistance();
            float a = ToTrack.GetCurrentActivation();
            
            var values = new UXFDataRow()
            {
                ("dist_x", dist.x),
                ("dist_y", dist.y),
                ("dist_z", dist.z),
                ("activation", a)
            };

            return values;
        }

        
    }
}

