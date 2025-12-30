using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace SensorimotorContingencies
{
    public class ControllerController  : MonoBehaviour
    {
        [SerializeField] GameObject interactor;
        [SerializeField] GameObject laser;
        [SerializeField] XRBaseController xr;

        private void Start()
        {
            if (xr == null)
            {
                xr = GetComponent<XRBaseController>();
            }
        }

        public void UseLaser(bool use)
        {
            laser.SetActive(use);
        }

        public void ShowInteractor(bool show)
        {
             interactor.layer = show ? 0 : 6;
        }

        public void UseInteractor(bool use)
        {
            interactor.SetActive(use);
        }

        public void Buzz()
        {
            xr.SendHapticImpulse(0.5f, 0.1f);
        }
    }
}