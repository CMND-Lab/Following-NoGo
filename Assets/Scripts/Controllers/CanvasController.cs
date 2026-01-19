using System.Collections;
using UnityEngine;
using System;
using UXF;
using System.Drawing.Drawing2D;

namespace FollowingNoGo
{
    public class CanvasController : MonoBehaviour
    {
        public ExperimentManager experimentManager;

        public GameObject backButton;
        public GameObject nextButton;
        public GameObject continueButton;

        public int numInstruction = 0;
        public int totInstructions;

        public Session session;
        public CanvasState canvasState;

        public CanvasInstructions canvasInstructions;


        private void Awake()
        {
            canvasInstructions.LoadInstructions();
            gameObject.SetActive(true);
            SetCanvasState(CanvasState.Init);
        }

        private void Start()
        {
        }

        public void SetupAfterInitialised()
        {
            SetCanvasState(CanvasState.Intro);
        }

        public void SetCanvasState(CanvasState state)
        {
            continueButton.SetActive(false);
            nextButton.SetActive(false);
            backButton.SetActive(false);

            switch (state)
            {
                case CanvasState.Init:
                    canvasInstructions.NewInstructions(canvasInstructions.initInstructionControllers, this);
                    break;

                case CanvasState.Intro:
                    nextButton.SetActive(true);
                    canvasInstructions.NewInstructions(canvasInstructions.introInstructionControllers, this);
                    break;

                case CanvasState.Baseline:
                    nextButton.SetActive(true);
                    canvasInstructions.NewInstructions(canvasInstructions.baselineInstructionControllers, this);
                    break;

                case CanvasState.Practice:
                    nextButton.SetActive(true);
                    canvasInstructions.NewInstructions(canvasInstructions.practiceInstructionControllers, this);
                    break;

                case CanvasState.InterTrial:
                    gameObject.SetActive(true);
                    canvasInstructions.NewInstructions(canvasInstructions.interTrialInstructionControllers, this);
                    break;

                case CanvasState.Experiment:
                    nextButton.SetActive(true);
                    canvasInstructions.NewInstructions(canvasInstructions.experimentInstructionControllers, this);
                    break;

                case CanvasState.Break:
                    experimentManager.DisableExperiment();
                    gameObject.SetActive(true);

                    continueButton.SetActive(true);

                    canvasInstructions.NewInstructions(canvasInstructions.breakInstructionsControllers, this);
                    break;

                case CanvasState.Halfway:
                    canvasInstructions.NewInstructions(canvasInstructions.halfwayInstructionsControllers, this);

                    StartCoroutine(HalfwaySequence());
                    break;

                case CanvasState.Finished:
                    canvasInstructions.NewInstructions(canvasInstructions.finishedInstructionsControllers, this);
                    break;
            }

            canvasState = state;
        }

        public void SetInstruction(string text)
        {
            canvasInstructions.SetCurrentInstruction(text);
        }

        public void ButtonClick(string buttonName)
        {
            switch (buttonName)
            {
                case "Next":
                    NextClick(); break;
                case "Back":
                    BackClick(); break;
                case "Continue":
                    ContinueClick(); break;
            }
        }

        public void LastInstruction()
        {
            nextButton.SetActive(false);
            continueButton.SetActive(true);
        }

        public void NextClick()
        {
            backButton.SetActive(true);
            canvasInstructions.NextInstruction(this);
        }

        public void FirstInstruction()
        {
            backButton.SetActive(false);
        }

        public void BackClick()
        {
            nextButton.SetActive(true);
            continueButton.SetActive(false);
            canvasInstructions.PreviousInstruction(this);
        }

        public void ContinueClick()
        {
            switch (canvasState)
            {
                case CanvasState.Intro:
                    SetCanvasState(CanvasState.Baseline); 
                    break;
                default:
                    //*** Start experiment ***//
                    gameObject.SetActive(false);

                    experimentManager.EnableExperiment();
                    break;
            }
        }

        IEnumerator HalfwaySequence()
        {
            yield return new WaitForSeconds(15);
            nextButton.SetActive(true);
        }

        public void EndOfBlock(TrialType blockType)
        {
            // Turn off Experiment, Turn on Canvas
            experimentManager.DisableExperiment();
            gameObject.SetActive(true);

            // Set Controller to UI mode

            switch (blockType)
            {
                case TrialType.Baseline:
                    // Set Canvas State to Practice
                    SetCanvasState(CanvasState.Experiment);
                    break;

                case TrialType.Practice:
                    // Set Canvas State to Experiment
                    SetCanvasState(CanvasState.Experiment);
                    break;

                case TrialType.Experiment:
                    EndOfExperimentBlock();
                    break;
            }
        }

        private void EndOfExperimentBlock()
        {
            // Disable controls
            backButton.SetActive(false);
            nextButton.SetActive(false);

            // Block 1 = baseline
            // Block 2 = practice
            if (session.currentBlockNum == session.blocks.Count)
            {
                SetCanvasState(CanvasState.Finished);
            }
            else
            {
                SetCanvasState(CanvasState.Break);
            }
        }

    }
}
public enum CanvasState
{
    Init,
    Intro,
    Demo,
    Baseline,
    Practice,
    Experiment,
    Break,
    Halfway,
    Finished,
    InterTrial
}
