using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class BlockSetting : MonoBehaviour
    {
        public int numTrials = 0;
        public bool randomiseTrialOrder = true;
        private TrialSetting[] trialSettings;
        private List<TrialSetting> unselectedTransformers = new List<TrialSetting>();

        private void Awake()
        {
            trialSettings = GetComponentsInChildren<TrialSetting>();
        }

        public TrialSetting PickUnselected()
        {
            if (unselectedTransformers.Count == 0)
            {
                unselectedTransformers = new List<TrialSetting>(trialSettings);
            }

            int randomObjectIndex = Random.Range(0, unselectedTransformers.Count);
            TrialSetting randomObject = unselectedTransformers[randomObjectIndex];

            unselectedTransformers.Remove(randomObject);

            return randomObject;
        }

        public List<TrialSetting> GetTrialList(bool shuffle = true)
        {
            List<TrialSetting> trials = new List<TrialSetting> ();

            if (numTrials <= trialSettings.Length)
            {
                trials = new List<TrialSetting>(trialSettings);
            }
            else
            {
                for (int i = 0; i < numTrials; i++)
                {
                    trials.Add(trialSettings[i % trialSettings.Length]);
                }
            }

            return trials;
        }
    }
}
