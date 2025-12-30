using UnityEngine;
using UnityEngine.Rendering;
using UXF;

namespace SensorimotorContingencies
{
    public class TransformDevice : aSensorimotorTransform
    {
        [Header("Inputs")]
        [SerializeField] InputDevice inputDevice;
        [SerializeField] InputMode inputMode;

        [SerializeField] bool use_x;
        [SerializeField] bool use_y;
        [SerializeField] float damping = 1.0f;

        public override void RecordTrialSettings(Trial trial)
        {
            string effectName = effectManager.gameObject.name;
            trial.result[effectName + "_transform"] = "device_position";
            trial.result[effectName + "_transform_controller"] = controllerInput.ToString();
            trial.result[effectName + "_transform_device"] = inputDevice.ToString();
            trial.result[effectName + "_transform_deviceMode"] = inputDevice.ToString();
            trial.result[effectName + "_transform_deviceX"] = use_x;
            trial.result[effectName + "_transform_deviceY"] = use_y;
        }

        protected override float TransformInputs(MotorController head, MotorController rightController, MotorController leftController)
        {
            MotorController controller = rightController;
            if (controllerInput == ControllerInput.Left) 
            {
                controller = leftController;
            }

            Vector3 activeVector = Vector3.zero;
            // Choose head/controller values
            if (inputDevice == InputDevice.Difference)
            {
                // Difference in world-space position
                if (inputMode == InputMode.Position)
                {
                    activeVector = head.GetPosition(false) - controller.GetPosition(false);
                }
                // Difference in velocity
                else if (inputMode == InputMode.Velocity)
                {
                    activeVector = head.GetVelocity() - controller.GetVelocity();
                }
            }
            else
            {
                if (inputDevice == InputDevice.Controller)
                {
                    // Position (from reference)
                    if (inputMode == InputMode.Position) { activeVector = controller.GetPosition(true); }

                    // Velocity
                    else if (inputMode == InputMode.Velocity) { activeVector = controller.GetVelocity(); }
                }

                else if (inputDevice == InputDevice.Head)
                {
                    // Position (from reference)
                    if (inputMode == InputMode.Position) { activeVector = head.GetPosition(true); }

                    // Velocity
                    else if (inputMode == InputMode.Velocity) { activeVector = head.GetVelocity(); }
                }
            }

            activeVector *= damping;

            // Get values from x/y axes
            float activeDistance = 0.0f;
            if (use_x && use_y) { activeDistance = (activeVector.y + activeVector.x) / 2; }
            else
            {
                if (use_x) { activeDistance = activeVector.x; }
                else if (use_y) { activeDistance = activeVector.y; }
            }

            return activeDistance;
        }
    }

    public enum InputDevice
    {
        Controller,
        Head,
        Difference
    }

    public enum InputMode
    {
        Position,
        Velocity
    }
}