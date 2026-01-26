using System.Collections;
using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class ExperimentManager : MonoBehaviour
    {
        public Session session;        
        [SerializeField] TaskController taskController;
        [SerializeField] LanternManager lanternManager;
        [SerializeField] CanvasController canvasController;

        public GameObject experiment;
        public ControllerController rightController;
        public ControllerController leftController;
        
        private void Awake()
        {
            DisableExperiment();
            session.gameObject.SetActive(true);
        }

        public void EnableExperiment()
        {
            taskController.ResetTrial();

            rightController.UseLaser(false);
            leftController.UseLaser(false);
        }

        public void DisableExperiment()
        {
            lanternManager.HideLanterns();

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

        public void StartOfTrial(Trial trial) 
        {
            canvasController.gameObject.SetActive(false);
            taskController.RunTrial(trial);
        }

        public void EndOfTrial(Trial trial) 
        {
            int currentTrialBlock = trial.block.number;

            if (trial != session.LastTrial)
            {
                int nextTrialBlock = session.NextTrial.block.number;

                if (currentTrialBlock != nextTrialBlock)
                {
                    canvasController.SetCanvasState(CanvasState.InterTrial);
                }
                else
                {
                    StartCoroutine(ResetTrial(2.5f));
                }
            }
        }

        IEnumerator ResetTrial(float delay)
        {
            yield return new WaitForSeconds(delay);
            taskController.ResetTrial();
        }

        public void StartOfBlock(Block block) {
        }

        public void EndOfBlock(Block block) // Is called at the end of each block of trials via the UXF Event system
        {
            Debug.Log("End of Block");

            // Get block type
            TrialType trialType = (TrialType)session.CurrentTrial.settings.GetObject("type");

            //Set EndTrial flag to false (this sometimes doesn't happen on the last trial via TaskController, not sure why)
            Debug.Log("End of " + trialType.ToString() + " block");

            canvasController.EndOfBlock(trialType);

            if (session.CurrentTrial == session.LastTrial && session.GetBlock(session.blocks.Count).lastTrial == session.CurrentTrial) // test whether some of this can be removed
            {
                StartCoroutine(EndOfExperiment());          
            }
        }
    }
}


