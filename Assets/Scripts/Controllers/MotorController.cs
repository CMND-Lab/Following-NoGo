using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [SerializeField] GameObject inputDevice;
    [SerializeField] Transform referenceObject;

    [SerializeField] Vector3 position;
    [SerializeField] Vector3 rotation;
    [SerializeField] Vector3 velocity;
    [SerializeField] float drag = 0.98f;

    private Vector3 lastPosition;

    private void Update()
    {
        // Tracker values
        position = gameObject.transform.position;
        rotation = transform.forward;

        // Calculate velocity
        Vector3 positionChange = position - lastPosition;
        Vector3 newVelocity = positionChange / Time.deltaTime;

        velocity *= drag;
        velocity += newVelocity.Abs();

        lastPosition = position;
    }

    public GameObject GetInputDevice()
    {
        return inputDevice;
    }

    public Vector3 GetPosition(bool relative)
    {
        if (relative && referenceObject != null)
        {
            return position - referenceObject.position;
        }
        return position;
    }

    public Vector3 GetRotation()
    {
        return rotation;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
}
