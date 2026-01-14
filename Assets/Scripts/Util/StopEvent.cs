using UnityEngine;

namespace FollowingNoGo
{
    public class StopEvent
    {
        float eventDelay;
        LanternLocaction eventTarget;

        public StopEvent(float eventDelay, LanternLocaction eventTarget)
        {
            this.eventDelay = eventDelay;
            this.eventTarget = eventTarget;
        }

        public float GetDelay() { return eventDelay; }
        public LanternLocaction GetTarget() {  return eventTarget; }
    }
}