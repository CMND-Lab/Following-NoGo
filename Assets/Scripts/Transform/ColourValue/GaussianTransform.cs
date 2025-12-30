using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianTransform : aActivateValue
{
    [Header("Curve")]
    [SerializeField] float mean = 0.0f;
    [SerializeField] float sigma = 1.0f;
    [SerializeField] float clamp = 100.0f;

    [Header("Randomisation")]
    [SerializeField] bool trigger = false;
    [SerializeField] float min = -1.0f;
    [SerializeField] float max = 1.0f;

    private void Update()
    {
       if (trigger)
        {
            Randomise();
            trigger = false;
        } 
    }

    public override float Activate(float value)
    {
        clamp = Mathf.Abs(clamp);
        if (clamp > 0.0f)
        {
            // Clamp value for repeating peaks
            float clampIncrement = value < 0.0f ? 2 * clamp : -2 * clamp;

            int iter_count = 0;
            while ((value > clamp || value < -clamp) && iter_count < 100) 
            {
                value += clampIncrement;
                iter_count++;
            }
        }
        

        return Gaussian(value);
    }

    public void Randomise()
    {
        if (min > max)
        {
            Debug.LogError("Bad randomisation values");
        }
        
        float newVal = Random.Range(min, max);
        mean = newVal;
    }

    public float Gaussian(float x)
    {
        // mean = peak of curve
        // sigma = standard dev
        float a = 1.0f;
        float b = Mathf.Exp(-Mathf.Pow(x - mean, 2) / (2.0f * sigma * sigma));
        return a * b;
    }
}
