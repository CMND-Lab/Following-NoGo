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
                "dist_x",
                "dist_y",
                "dist_z",
                "activation"
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

