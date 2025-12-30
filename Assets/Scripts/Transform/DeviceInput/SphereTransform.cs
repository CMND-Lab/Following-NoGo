using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UXF;

namespace SensorimotorContingencies
{
    public class SphereTransform : aSensorimotorTransform
    {
        [SerializeField] GameObject shapeOrigin;

        public override void RecordTrialSettings(Trial trial)
        {
            string effectName = effectManager.gameObject.name;
            trial.result[effectName + "_transform"] = "sphere";
            trial.result[effectName + "_transform_controller"] = controllerInput.ToString();
            trial.result[effectName + "_transform_shapeOrigin"] = shapeOrigin.gameObject.name;
        }

        protected override float TransformInputs(MotorController head, MotorController rightController, MotorController leftController)
        {
            MotorController controller = rightController;
            if (controllerInput == ControllerInput.Left)
            {
                controller = leftController;
            }

            if (shapeOrigin == null)
            {
                shapeOrigin = head.GetInputDevice();
            }
            Vector3 headPosition = shapeOrigin.transform.position;
            Vector3 headDirection = shapeOrigin.transform.forward.normalized;

            // Calculate point closest to controller
            Vector3 toController = controller.transform.position - headPosition;

            // Debug
            Debug.DrawLine(headPosition, controller.transform.position, Color.magenta);

            return toController.magnitude;
        }
    }
}