using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace SensorimotorContingencies
{
    public class CylinderTransform : aSensorimotorTransform
    {
        [SerializeField] GameObject shapeOrigin;

        public override void RecordTrialSettings(Trial trial)
        {
            string effectName = effectManager.gameObject.name;
            trial.result[effectName + "_transform"] = "cylinder";
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
            float projection = Vector3.Dot(toController, headDirection);

            Vector3 closestPoint = headPosition + headDirection * projection;

            // Get distance to closest point
            Vector3 controllerDistance = controller.transform.position - closestPoint;
            Vector3 headDistance = headPosition - closestPoint;


            // Debug
            Debug.DrawLine(headPosition, closestPoint, Color.magenta);
            Debug.DrawLine(controller.transform.position, closestPoint, Color.cyan);

            Debug.Log("Controller Dist: " + controllerDistance.magnitude);
            Debug.Log("Head Dist: " + headDistance.magnitude);

            return controllerDistance.magnitude;
        }
    }
}