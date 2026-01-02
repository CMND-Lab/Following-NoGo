using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXF;


namespace FollowingNoGo
{
    public class TaskController : MonoBehaviour
    {
        public LanternManager lanternManager;

        [SerializeField] bool endTrial = false;
        [SerializeField] bool forceStop = false;

        public bool useTimer;
        public TimeManager timeManager;

        [SerializeField] ControllerController rightController;
        [SerializeField] ControllerController leftController;

        public TrialSetting trialSetting;

        // Called at the start of each trial via UFX
        public void RunTrial(Trial trial)
        {
            // Run trial
            StartCoroutine(TaskTrialSequence(trial));
        }

        public void ResetTrial() 
        {           
            Debug.Log("Resetting");

            trialSetting = null;

            // reset endTrial flag for next 
            endTrial = false;
            forceStop = false;

            rightController.ShowInteractor(true);
            leftController.ShowInteractor(true);

            lanternManager.ShowLanterns();
            lanternManager.EnableStart();
        }

        public void SetupTrial(Trial trial)
        {
            // Sensorimotor contingency
            trialSetting = (TrialSetting)trial.settings.GetObject("settings");

            // Set trial duration
            timeManager.SetDuration(trialSetting.trialDuration);
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

        // Coroutine for the trial behaviour
        IEnumerator TaskTrialSequence(Trial trial)
        {
            endTrial = false;

            // Setup trial
            SetupTrial(trial);

            // Start trial timer
            if (useTimer) { timeManager.BeginCountdown(); }

            while (!endTrial) { yield return null; }
            timeManager.StopCountdown();
            
            lanternManager.HideLanterns();
            // end current trial
            trial.End();
        }
    }
}
