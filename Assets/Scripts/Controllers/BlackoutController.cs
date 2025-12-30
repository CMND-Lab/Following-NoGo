using UnityEngine;

namespace SensorimotorContingencies
{
    public class BlackoutController : MonoBehaviour
    {
        private bool isBlackout = false;
        [SerializeField] Material blackoutMat;

        [SerializeField] bool isAnimating = false;
        [SerializeField] float fadeSpeed = 0.5f;
        private float changeAmount = 0.0f;


        private void Awake()
        {
            if (blackoutMat == null) 
            { 
                blackoutMat = GetComponent<MeshRenderer>().material;
            }
        }

        private void Update()
        {
            if (isAnimating && changeAmount != 0.0f)
            {
                float matAlpha = blackoutMat.color.a;
                float newAlpha = Mathf.Clamp(matAlpha + changeAmount * Time.deltaTime, 0.0f, 1.0f);
                Color newColour = new Color(0.0f, 0.0f, 0.0f, newAlpha);
                blackoutMat.color = newColour;

                if (newAlpha <= 0.0f || newAlpha >= 1.0f)
                {
                    isAnimating = false;
                    isBlackout = blackoutMat.color.a >= 1.0f;
                }
            }
        }

        public void TriggerAnimation(bool set)
        {
            isAnimating = true;
            changeAmount = set ? Mathf.Abs(fadeSpeed): -Mathf.Abs(fadeSpeed);
        }

        public bool NotBlackedOut() { return !isBlackout; }
        public bool IsBlackedOut() {  return isBlackout; }
        public void BlackoutOn() { isBlackout = true; }
        public void BlackoutOff() { isBlackout = false; }
    }
}



