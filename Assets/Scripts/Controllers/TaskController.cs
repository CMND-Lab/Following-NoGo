using System.Collections;
using UnityEngine;
using UXF;


namespace FollowingNoGo
{
    public class TaskController : MonoBehaviour
    {
        public LanternManager lanternManager;

        [SerializeField] LanternAnimation lanternAnimation;
        [SerializeField] bool endTrial = false;
        [SerializeField] bool forceStop = false;

        public TimeManager timeManager;

        [SerializeField] ControllerController rightController;
        [SerializeField] ControllerController leftController;

        public TrialSetting trialSetting;
        private bool timingStarted = false;

        // Called at the start of each trial via UFX
        public void RunTrial(Trial trial)
        {
            // Run trial
            SetupTrial(trial);
            StartCoroutine(TaskTrialSequence(trial));
        }

        public void ResetTrial() 
        {           
            Debug.Log("Resetting");

            trialSetting = null;

            // reset endTrial flag for next 
            endTrial = false;
            forceStop = false;
            timingStarted = false;

            rightController.ShowInteractor(true);
            leftController.ShowInteractor(true);

            lanternManager.ResetLanterns();
            lanternManager.ShowLanterns();
            lanternManager.EnableStart(true);
        }

        public void SetupTrial(Trial trial)
        {
            // Sensorimotor contingency
            trialSetting = (TrialSetting)trial.settings.GetObject("settings");

            // Set trial duration
            timeManager.SetTrial(trialSetting);

            // Lantern reactivity
            lanternManager.UseChangingColour(true);
        }

        public void TimerEnd()
        {
            endTrial = true;
        }

        public void ForceEnd()
        {
            forceStop = true;
            endTrial = true;
        }

        private readonly object _lockObject = new object();
        public void BeginTrialTiming()
        {
            lock (_lockObject)
            {
                if (timingStarted == false)
                {
                    Debug.Log("Starting trial timing...");
                    timingStarted = true;

                    // Lantern events
                    timeManager.RunEvents();

                    // Start trial timer
                    timeManager.BeginCountdown();
                }
            }
        }

        // Coroutine for the trial behaviour
        IEnumerator TaskTrialSequence(Trial trial)
        {
            endTrial = false;
            lanternManager.DoAnimation(lanternAnimation);

            while (!endTrial) { yield return null; }
            timeManager.StopCountdown();
            
            lanternManager.HideLanterns();
            // end current trial
            trial.End();
        }
    }
}
