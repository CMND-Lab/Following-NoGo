using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXF;


namespace SensorimotorContingencies
{
    public class TaskController : MonoBehaviour
    {
        public StartingPointTrialController startingPoint;

        [SerializeField] bool endTrial = false;
        [SerializeField] bool forceStop = false;

        public bool useTimer;
        public TimeManager timeManager;

        [SerializeField] GameObject defaultRoom;
        [SerializeField] BlackoutController blackoutController;
        [SerializeField] ControllerController rightController;
        [SerializeField] ControllerController leftController;
        public MotorInputManager inputManager;

        public TrialSetting trialSetting;

        public void Awake()
        {
            blackoutController.gameObject.SetActive(true);
            blackoutController.TriggerAnimation(false);
            defaultRoom.gameObject.SetActive(true);
        }

        // Called at the start of each trial via UFX
        public void RunTrial(Trial trial)
        {
            // Run trial
            StartCoroutine(TaskTrialSequence(trial));
        }

        public void ResetTrial() {
            inputManager.DisableTransform();
            if (trialSetting != null)
            {
                trialSetting.DeactivateEffects();
            }
            trialSetting = null;

            // reset endTrial flag for next 
            endTrial = false;
            forceStop = false;

            defaultRoom.SetActive(true);
            StartCoroutine(FadeInReset());
        }

        private IEnumerator FadeInReset()
        {
            blackoutController.TriggerAnimation(false);
            rightController.ShowInteractor(true);
            leftController.ShowInteractor(true);

            yield return new WaitUntil(blackoutController.NotBlackedOut);

            startingPoint.gameObject.SetActive(true);
            startingPoint.ResetStartOrb();

            rightController.UseInteractor(true);
            leftController.UseInteractor(true);
        }

        public void SetupTrial(Trial trial)
        {
            // Enable interaction
            rightController.ShowInteractor(false);
            leftController.ShowInteractor(false);

            // Sensorimotor contingency
            trialSetting = (TrialSetting)trial.settings.GetObject("settings");
            List<aSensorimotorTransform> trialTransforms = trialSetting.GetTransforms();
            trialSetting.ActivateEffects();

            // Record active transformations
            List<string> transformNames = trialTransforms.Select(x => x.gameObject.name).ToList();
            Debug.Log("Transforms: " + string.Join(", ", transformNames));
            trial.result["transform"] = string.Join(", ", transformNames);

            // Set trial duration
            timeManager.SetDuration(trialSetting.trialDuration);

            // Activate control
            inputManager.gameObject.SetActive(true);
            inputManager.SetTransform(trialTransforms);

            defaultRoom.SetActive(false);
        }

        public void ExitStartOrb()
        {
            
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

            // Fade in from black
            blackoutController.TriggerAnimation(false);
            yield return new WaitUntil(blackoutController.NotBlackedOut);
            trial.result["time_end_fadein"] = Time.time;

            // Start trial timer
            if (useTimer) { timeManager.BeginCountdown(); }

            while (!endTrial) { yield return null; }
            timeManager.StopCountdown();
            
            // Fade to black
            trial.result["time_start_fadeout"] = Time.time;

            blackoutController.TriggerAnimation(true);
            yield return new WaitUntil(blackoutController.IsBlackedOut);
            
            ResetTrial();
            // end current trial
            trial.End();
        }
    }
}
