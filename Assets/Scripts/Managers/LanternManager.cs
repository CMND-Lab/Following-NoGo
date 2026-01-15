using UnityEngine;
using UnityEngine.UIElements;
using UXF;

namespace FollowingNoGo
{
    public class LanternManager : MonoBehaviour
    {
        [SerializeField] LanternController leftLantern;
        [SerializeField] LanternController rightLantern;

        [SerializeField] CanvasController canvasController;
        [SerializeField] TaskController taskController;

        private bool leftIsReady = false;
        private bool rightIsReady = false;

        private bool initiateTrial = false;

        public void ResetLanterns()
        {
            leftIsReady = false;
            rightIsReady = false;

            rightLantern.Reset();
            leftLantern.Reset();
        }

        public void ShowLanterns()
        {
            gameObject.SetActive(true);
            leftLantern.gameObject.SetActive(true);
            rightLantern.gameObject.SetActive(true);
        }

        public void EnableStart(bool initiatingTrial = true)
        {
            initiateTrial = initiatingTrial;

            leftLantern.UseStart(true);
            rightLantern.UseStart(true);
        }

        public void HideLanterns()
        {
            leftLantern.gameObject.SetActive(false);
            rightLantern.gameObject.SetActive(false);
        }

        public void DoAnimation(LanternAnimation animation)
        {
            rightLantern.TriggerAnimation(animation);
            leftLantern.TriggerAnimation(animation);
        }

        public void EnterLantern(LanternLocaction location)
        {
            Debug.Log("Ready: " + location.ToString());
            if (location == LanternLocaction.Left)
            {
                leftIsReady = true;
            }
            else if (location == LanternLocaction.Right)
            {
                rightIsReady = true;
            }

            if (leftIsReady && rightIsReady)
            {
                Debug.Log("Both lanterns ready...");
                leftLantern.UseStart(false);
                leftLantern.UseChangingColour(true);

                rightLantern.UseStart(false);
                rightLantern.UseChangingColour(true);

                if (initiateTrial)
                {
                    Session.instance.BeginNextTrial();
                } else
                {
                    canvasController.ContinueClick();
                }
            }
        }

        public void ExitLantern(LanternLocaction location)
        {
            Debug.Log("Exit lantern: " + location.ToString());
            if (location == LanternLocaction.Left)
            {
                leftIsReady = false;
            }
            else if (location == LanternLocaction.Right)
            {
                rightIsReady = false;
            }
        }

        public void PauseAnimation(LanternLocaction target)
        {
            if (target == LanternLocaction.Right || target == LanternLocaction.Both)
            {
                rightLantern.PauseAnimation();
            }
            else if (target == LanternLocaction.Left || target == LanternLocaction.Both)
            {
                leftLantern.PauseAnimation();
            }
        }

        public void FinishCycle()
        {
            if (Session.instance.InTrial)
            {
                taskController.BeginTrialTiming();
            }
        }
    }

    public enum LanternLocaction
    {
        Right,
        Left,
        None,
        Both
    }
}