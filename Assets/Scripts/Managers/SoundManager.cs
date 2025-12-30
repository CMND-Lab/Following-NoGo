using UnityEditor.Overlays;
using UnityEngine;

namespace SensorimotorContingencies
{
    public class SoundManager : aTransformEffect
    {
        [SerializeField] AudioSource baseAudio;
        [SerializeField] Animator soundAnimation;

        [Header("Effect Settings")]
        [SerializeField] SoundEffect effect;
        [SerializeField] float minValue = 0;
        [SerializeField] float maxValue = 2;

        private Animator animator;

        public override void Activate(bool active)
        {
            gameObject.SetActive(active);
            animator = GetComponent<Animator>();

            if (effect == SoundEffect.Pitch)
            {
                // Fade-in/Fade-out volume
                animator.enabled = true;
                soundAnimation.SetBool("On", active);
            }
            else if (effect == SoundEffect.Volume)
            {
                // Disable volume animation to allow manual control
                animator.enabled = false;
            }
        }

        public override void Transform(float value)
        {
            currentActivation = value;
            // Value is 0 - 1
            // BaseAudio starts at pitch = 1
            // Pitch should vary between 0 - 2

            float activationValue = Mathf.Abs(value) * (maxValue - minValue) + minValue;
            
            if (effect == SoundEffect.Pitch)
            {
                baseAudio.pitch = activationValue;
            } 
            else if (effect == SoundEffect.Volume)
            {
                baseAudio.volume = activationValue;
            }
        }
    }

    public enum SoundEffect
    {
        Pitch,
        Volume
    }
}