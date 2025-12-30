using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SensorimotorContingencies
{
    public class TrialSetting : MonoBehaviour
    {
        public float trialDuration = 15.0f;
        [SerializeField] List<aSensorimotorTransform> transforms;

        public List<aSensorimotorTransform> GetTransforms() { return transforms; }

        public void ActivateEffects()
        {
            foreach (aSensorimotorTransform t in transforms)
            {
                t.UseEffect(true);
            }
        }

        public void DeactivateEffects()
        {
            foreach(aSensorimotorTransform t in transforms)
            {
                t.UseEffect(false);
            }
        }

        public override string ToString()
        {
            return string.Join(", ", transforms.Select(x => x.gameObject.name));
        }
    }
}