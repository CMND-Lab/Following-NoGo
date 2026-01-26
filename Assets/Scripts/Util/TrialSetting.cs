using System.Collections.Generic;

namespace FollowingNoGo
{
    public class TrialSetting
    {
        private float trialDuration = 15.0f;
        private List<StopEvent> trialEvents = new List<StopEvent>();

        public TrialSetting(float duration)
        {
            trialDuration = duration;
        }

        public TrialSetting(float duration, List<StopEvent> events)
        {
            trialDuration = duration;
            trialEvents = events;
        }

        public float GetDuration()
        {
            return trialDuration;
        }

        public List<StopEvent> GetEvents()
        {
            return trialEvents;
        }

        public override string ToString()
        {
            string s = "Duration: " + trialDuration + "\n";
            foreach (StopEvent e in trialEvents)
            {
                s += "\t" + e.GetDelay() + " - " + e.GetTarget().ToString() + "\n";
            }
            return s;
        }
    }
}