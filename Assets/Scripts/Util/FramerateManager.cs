using UnityEngine;

public class FramerateManager : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 90;

        OVRPlugin.systemDisplayFrequency = 90f;
    }
}