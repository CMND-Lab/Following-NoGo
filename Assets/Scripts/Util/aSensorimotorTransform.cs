using UnityEngine;
using UXF;

namespace SensorimotorContingencies
{
    public abstract class aSensorimotorTransform : MonoBehaviour
    {
        public aTransformEffect effectManager;
        public aActivateValue activationFunction;
        [SerializeField] protected ControllerInput controllerInput = ControllerInput.Right;
        private float currentActivation;
        public float GetCurrentActivation() {  return currentActivation; }

        private void Awake()
        {
            if (activationFunction == null) { activationFunction = GetComponent<aActivateValue>(); }
        }

        public void Activate(MotorController headTransform, MotorController rightControllerTransform, MotorController leftControllerTransform)
        {
            // Get motor activation value
            float motorActivation = TransformInputs(headTransform, rightControllerTransform, leftControllerTransform);

            // Pass motor activation to signal transformation
            currentActivation = activationFunction.Activate(motorActivation);

            // Give activation value to effect manager
            effectManager.Transform(currentActivation);
        }
        protected abstract float TransformInputs(MotorController headTransform, MotorController rightControllerTransform, MotorController leftControllerTransform);
        public void UseEffect(bool active)
        {
            effectManager.Activate(active);
        }
        public abstract void RecordTrialSettings(Trial trial);
    }

    public enum ControllerInput
    {
        Right,
        Left
    }
}