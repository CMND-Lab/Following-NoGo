using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UXFExamples;

namespace FollowingNoGo
{
    public class StartingPointTrialController : aStartingPoint
    {
        protected override IEnumerator HoldingSequence()
        {
            state = StartingStateVR.GetReady;
            yield return new WaitForSeconds(preHoldTime);

            yield return new WaitForSeconds(holdTime);

            state = StartingStateVR.Go;

            lanternManager.EnterLantern(location);
        }
    }
}

