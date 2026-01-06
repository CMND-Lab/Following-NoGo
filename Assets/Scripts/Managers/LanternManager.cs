using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class LanternManager : MonoBehaviour
    {
        [SerializeField] LanternController leftLantern;
        [SerializeField] LanternController rightLantern;

        [SerializeField] TaskController taskController;

        private bool leftIsReady = false;
        private bool rightIsReady = false;

        public void Reset()
        {
            leftIsReady = false;
            rightIsReady = false;
        }

        public void ShowLanterns()
        {
            leftLantern.gameObject.SetActive(true);
            rightLantern.gameObject.SetActive(true);
        }

        public void EnableStart()
        {
            leftLantern.UseStart(true);
            rightLantern.UseStart(true);
        }

        public void HideLanterns()
        {
            leftLantern.gameObject.SetActive(false);
            rightLantern.gameObject.SetActive(false);
        }

        public void EnterLantern(LanternLocaction location)
        {
            Debug.Log("Enter lantern: " + location.ToString());
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

                Session.instance.BeginNextTrial();
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
    }

    public enum LanternLocaction
    {
        Right,
        Left,
        None
    }
}