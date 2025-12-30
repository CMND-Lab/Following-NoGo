using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UXF;

namespace SensorimotorContingencies
{
    public class MotorInputManager : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] MotorController leftControllerObject;
        [SerializeField] MotorController rightControllerObject;
        [SerializeField] MotorController headObject;

        [SerializeField] bool doTransform = false;
        [SerializeField] List<aSensorimotorTransform> activeTransforms;

        private void Awake()
        {
            doTransform = false;
        }

        private void Start()
        {
            InputTracking.trackingLost += OnTrackingLost;
            InputTracking.trackingAcquired += OnTrackingAcquired;
        }

        private void OnTrackingLost(XRNodeState obj)
        {
            Debug.Log("Lost tracking: " + obj.nodeType);
        }

        private void OnTrackingAcquired(XRNodeState obj)
        {
            Debug.Log("Tracking: " + obj.nodeType);
        }


        // Update is called once per frame
        private void Update()
        {
            if (doTransform)
            {
                TransformInputs();
            }
        }

        public void DisableTransform()
        {
            doTransform = false;
        }

        public void SetTransform(List<aSensorimotorTransform> transforms)
        {
            activeTransforms = transforms;
            doTransform = true;
        }

        public void TransformInputs()
        {
            if (activeTransforms == null || activeTransforms.Count == 0)
            {
                Debug.LogError("No active transform controller!");
                return;
            }
            foreach (aSensorimotorTransform t in activeTransforms)
            {
                t.Activate(headObject, rightControllerObject, leftControllerObject);
            }
        }
    }
}