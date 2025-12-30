using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;
using UXF;
using UnityEditor.XR.LegacyInputHelpers;

public class ExperimentLocation : MonoBehaviour
{
    [SerializeField] GameObject experiment;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform cameraOffset;

    [SerializeField] GameObject[] adjustLocations;
    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.gameObject.transform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            RecordObjectPositions();
        }
    }

    private void RecordObjectPositions() {
        bool experimentIsActive = experiment.activeSelf;
        experiment.SetActive(true);

        RecordBounds[] position_recorders = FindObjectsOfType<RecordBounds>(true);

        foreach (RecordBounds recorder in position_recorders) {
                
            bool recorderIsActive = recorder.gameObject.activeSelf;
            recorder.gameObject.SetActive(true);
                
            recorder.Record();

            recorder.gameObject.SetActive(recorderIsActive);
        }

        experiment.SetActive(experimentIsActive);
    }

    public float AdjustExperimentHeight()
    {
        Vector3 headPosition = cameraTransform.position;
        Debug.Log("Head position: " + headPosition.ToString());

        foreach (GameObject adjust in adjustLocations)
        {
            Transform object_pos = adjust.transform;
            Debug.Log("Current position for " + adjust.name + ": " + object_pos.position.ToString("F4"));

            object_pos.position = new Vector3(object_pos.position.x, headPosition.y, object_pos.position.z);
            Debug.Log("New position for " + adjust.name + ": " + object_pos.position.ToString("F4"));
        }

        RecordObjectPositions();

        Vector3 offsetPosition = new Vector3(-headPosition.x, 0.0f, -headPosition.z);
        cameraOffset.position += offsetPosition;

        Session.instance.participantDetails["height"] = headPosition.y;
        return headPosition.y;
    }
}