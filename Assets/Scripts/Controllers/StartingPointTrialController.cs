using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;
using UXFExamples;

namespace SensorimotorContingencies
{
    public class StartingPointTrialController : MonoBehaviour
    {
        public TaskController taskController;
        public CanvasController canvasController;
        public Session session;
        private StartingStateVR state = StartingStateVR.Waiting;
        private Coroutine cueCoroutine;
        private StartingPointController pointController;

        [SerializeField] bool hideAfterStartTrial = true;
        [SerializeField] BlackoutController blackoutController;

        [Header("Timing")]
        [SerializeField] float preHoldTime;

        private void Awake()
        {
            GetPointController();
        }

        public void ShowOrb()
        {
            gameObject.SetActive(true);
            pointController.ToggleCollider(false);
            pointController.ToggleRenderer(true);
        }

        public StartingPointController GetPointController()
        {
            if (pointController == null)
            {
                pointController = gameObject.GetComponent<StartingPointController>();
            }
            return pointController;
        }

        IEnumerator TrialCueSequence()
        {
            state = StartingStateVR.GetReady;
            yield return new WaitForSeconds(preHoldTime);
            
            blackoutController.TriggerAnimation(true);
            yield return new WaitUntil(blackoutController.IsBlackedOut);

            state = StartingStateVR.Go;

            if (hideAfterStartTrial)
            {
                pointController.ToggleRenderer(false);
            }

            session.BeginNextTrial();
        }

        public void ResetStartOrb()
        {
            state = StartingStateVR.Waiting;
            GetPointController().ResetState();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            switch (state)
            {
                case StartingStateVR.Waiting:
                    cueCoroutine = StartCoroutine(TrialCueSequence());
                    break;
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            switch (state)
            {
                case StartingStateVR.GetReady:
                    StopCoroutine(cueCoroutine);
                    blackoutController.TriggerAnimation(false);
                    ResetStartOrb();
                    break;

                case StartingStateVR.Go:
                    pointController.ToggleCollider(false);
                    pointController.ToggleRenderer(false);

                    taskController.ExitStartOrb();
                    break;
            }
        }
    }
}

