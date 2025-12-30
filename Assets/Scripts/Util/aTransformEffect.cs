using UnityEngine;

public abstract class aTransformEffect: MonoBehaviour
{
    [SerializeField] protected float currentActivation;
    public float GetCurrentActivation() {  return currentActivation; }
    
    // Modify the environment according to an activation value
    public abstract void Transform(float value);

    // Called at the start of each trial by the relevant aSensorimotorTransform
    public virtual void Activate(bool active)
    {
        gameObject.SetActive(active);
    }
}
