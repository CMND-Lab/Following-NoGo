using UnityEngine;
using System;
using UXF;
using System.Linq;
using System.Collections.Generic;


namespace FollowingNoGo
{
    public class CanvasInstructions : MonoBehaviour
    {
        [SerializeField] GameObject initInstructions;
        public InstructionController[] initInstructionControllers;

        [SerializeField] GameObject introInstructions;
        public InstructionController[] introInstructionControllers;

        [SerializeField] GameObject baselineInstructions;
        public InstructionController[] baselineInstructionControllers;
        
        [SerializeField] GameObject practiceInstructions;
        public InstructionController[] practiceInstructionControllers;
        
        [SerializeField] GameObject experimentInstructions;
        public InstructionController[] experimentInstructionControllers;

        [SerializeField] GameObject interTrialInstructions;
        public InstructionController[] interTrialInstructionControllers;

        [SerializeField] GameObject breakInstructions;
        public InstructionController[] breakInstructionsControllers;

        [SerializeField] GameObject halfwayInstructions;
        public InstructionController[] halfwayInstructionsControllers;

        [SerializeField] GameObject finishedInstructions;
        public InstructionController[] finishedInstructionsControllers;

        private List<InstructionController> activeInstructions;
        private int numInstruction;
        private int lastInstruction;

        private void Awake()
        {
            LoadInstructions();
        }

        public void LoadInstructions()
        {
            initInstructionControllers = initInstructions.GetComponentsInChildren<InstructionController>(true);
            introInstructionControllers = introInstructions.GetComponentsInChildren<InstructionController>(true);
            baselineInstructionControllers = baselineInstructions.GetComponentsInChildren<InstructionController>(true);
            practiceInstructionControllers = practiceInstructions.GetComponentsInChildren<InstructionController>(true);
            experimentInstructionControllers = experimentInstructions.GetComponentsInChildren<InstructionController>(true);
            interTrialInstructionControllers = interTrialInstructions.GetComponentsInChildren<InstructionController>(true);
            breakInstructionsControllers = breakInstructions.GetComponentsInChildren<InstructionController>(true);
            halfwayInstructionsControllers = halfwayInstructions.GetComponentsInChildren<InstructionController>(true);
            finishedInstructionsControllers = finishedInstructions.GetComponentsInChildren<InstructionController>(true);
        }

        public void NextInstruction(CanvasController caller)
        {
            if (numInstruction < lastInstruction)
            {
                activeInstructions[numInstruction].Deactivate();
                numInstruction++;
            };
            if (numInstruction == lastInstruction)
            {
                caller.LastInstruction();
            }

            activeInstructions[numInstruction].Activate();
        }

        public void PreviousInstruction(CanvasController caller)
        {
            if (numInstruction > 0)
            {
                activeInstructions[numInstruction].Deactivate();
                numInstruction--;
            }
            if (numInstruction == 0)
            {
                caller.FirstInstruction();
            }

            activeInstructions[numInstruction].Activate();
        }

        public void SetCurrentInstruction(string text)
        {
            activeInstructions[numInstruction].SetInstructionText(text);
        }

        public bool IsLastInstruction()
        {
            return numInstruction == lastInstruction;
        }

        public void NewInstructions(InstructionController[] instructionList, CanvasController caller)
        {
            // Hide existing instructions
            if (activeInstructions != null && activeInstructions.Count > numInstruction)
            {
                activeInstructions[numInstruction].Deactivate();
            }

            Debug.Log("Assigning new instructions...");

            activeInstructions = instructionList.ToList();
            numInstruction = 0;
            lastInstruction = activeInstructions.Count-1;

            if (numInstruction == lastInstruction)
            {
                caller.LastInstruction();
            }
            activeInstructions[numInstruction].Activate();
        }
    }
}    
    
