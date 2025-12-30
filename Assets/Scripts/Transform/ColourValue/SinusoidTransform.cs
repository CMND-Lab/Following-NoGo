using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidTransform : aActivateValue
{
    [SerializeField] float nestingConstant = 1.5f;

    public override float Activate(float value)
    {
        return Mathf.Pow(Mathf.Sin(nestingConstant * value), 4);
    }
}
