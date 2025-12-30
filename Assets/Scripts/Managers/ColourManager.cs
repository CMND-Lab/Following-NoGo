using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace SensorimotorContingencies
{
    public class ColourManager : aTransformEffect
    {
        [SerializeField] Material transformColour;

        [SerializeField] Color baseColour;
        [SerializeField] Color peakColour;

        [SerializeField] float emissionStrength = 0.25f;
        [SerializeField] float darknessLerp = 0.25f;

        public override void Transform(float activationValue)
        {
            currentActivation = activationValue;
            Color activatedColour = Color.Lerp(baseColour, peakColour, activationValue);
            SetColour(activatedColour);
        }

        public void SetColour(Color c)
        {
            Color blackenedColour = Color.Lerp(c, Color.black, darknessLerp);

            transformColour.color = blackenedColour;

            // !!Not sure if this works
            transformColour.SetColor("_EmissionColor", c * emissionStrength);
            transformColour.EnableKeyword("_EMISSION");
        }
    }
}