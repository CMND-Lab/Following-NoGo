using System.Collections;
using UnityEngine;

namespace FollowingNoGo
{
    public class StartingPointDemoController : aStartingPoint
    {
        protected override IEnumerator HoldingSequence()
        {
            state = StartingStateVR.GetReady;
            yield return new WaitForSeconds(preHoldTime);

            // Trigger for trial lead-in
            yield return new WaitForSeconds(holdTime);

            state = StartingStateVR.Go;


            canvasController.NextClick();
        }
    }
}

