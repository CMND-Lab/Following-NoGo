using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace SensorimotorContingencies
{
    public class ActivationValueTracker : Tracker
    {
        [SerializeField] aTransformEffect trackActivation;

        public override string MeasurementDescriptor => "activation";
            
        public override IEnumerable<string> CustomHeader => new string[]
        {
            "activation"
        };
        

        protected override UXFDataRow GetCurrentValues()
        {
            float activationValue = trackActivation.GetCurrentActivation();
            
            // Empty data if not active
            var values = new UXFDataRow()
            {
                ("activation", trackActivation.gameObject.activeSelf ? activationValue : "")
            };

            return values;
        }
    }
}

