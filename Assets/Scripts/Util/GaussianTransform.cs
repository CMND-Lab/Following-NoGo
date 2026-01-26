using UnityEngine;

public class GaussianTransform : MonoBehaviour
{
    [Header("Curve")]
    [SerializeField] float mean = 0.0f;
    [SerializeField] float sigma = 1.0f;

    public float Gaussian(float x)
    {
        // mean = peak of curve
        // sigma = standard dev
        float a = 1.0f;
        float b = Mathf.Exp(-Mathf.Pow(x - mean, 2) / (2.0f * sigma * sigma));
        return a * b;
    }
}
