using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace SensorimotorContingencies
{
    public class ExperimentManager : MonoBehaviour
    {
        public Session session;        
        [SerializeField] TaskController taskController;
        [SerializeField] CanvasController canvasController;
        [SerializeField] List<GameObject> effectManagers;

        public GameObject experiment;
        public ControllerController rightController;
        public ControllerController leftController;
        
        private void Awake()
        {
            DisableEffects();
            taskController.inputManager.DisableTransform();

            // Enable user interaction
            rightController.UseInteractor(true);
            leftController.UseInteractor(true);
            rightController.UseLaser(true);
            leftController.UseLaser(true);
            
            session.gameObject.SetActive(true);
        }

        public void DisableEffects()
        {
            foreach (GameObject obj in effectManagers)
            {
                obj.SetActive(false);
            }
        }

        public void EnableExperiment()
        {
            taskController.ResetTrial();
            rightController.UseLaser(false);
            leftController.UseLaser(false);
        }

        public void DisableExperiment()
        {
            DisableEffects();

            // Reset environment
            taskController.ResetTrial();
            taskController.startingPoint.gameObject.SetActive(false);

            // Enable user interaction
            rightController.UseInteractor(true);
            leftController.UseInteractor(true);
            
            rightController.UseLaser(true);
            leftController.UseLaser(true);
        }

        IEnumerator EndOfExperiment()
        {
            Debug.Log("Finalizing Session");
            yield return new WaitForSeconds(10.0f);
            session.End();
        }

        public void StartOfTrial(Trial trial) {
            canvasController.gameObject.SetActive(false);
            taskController.RunTrial(trial);
        }

        public void EndOfTrial(Trial trial) {
            canvasController.SetCanvasState(CanvasState.InterTrial);
        }

        public void StartOfBlock(Block block) {
        }

        public void EndOfBlock(Block block) // Is called at the end of each block of trials via the UXF Event system
        {
            Debug.Log("End of Block");

            // Get block type
            TrialType trialType = (TrialType)session.CurrentTrial.settings.GetObject("type");

            //Set EndTrial flag to false (this sometimes doesn't happen on the last trial via TaskController, not sure why)
            taskController.ResetTrial();
            Debug.Log("End of " + trialType.ToString() + " block");

            canvasController.EndOfBlock(trialType);

            if (session.CurrentTrial == session.LastTrial && session.GetBlock(session.blocks.Count).lastTrial == session.CurrentTrial) // test whether some of this can be removed
            {
                StartCoroutine(EndOfExperiment());          
            }
        }
    }
}


