using Ookii.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using UnityEngine;
using Color = UnityEngine.Color;

namespace FollowingNoGo
{
    [RequireComponent(typeof(GaussianTransform))]
    public class LanternController: MonoBehaviour
    {
        [SerializeField] LanternManager manager;
        [SerializeField] GameObject lanternCenter;
        [SerializeField] GameObject controllerObject;
        private aStartingPoint startingPointController;

        [Header("Colour Options")]
        [SerializeField] bool updateColour = false; // Used in update function
        // Light settings
        private Light lanternLight;
        [SerializeField] float maxLightIntensity = 0.5f;
        
        // Non-changing colour
        [SerializeField] Material defaultMat;
        
        // Changing colour
        [SerializeField] Material changingMat;
        [SerializeField] Color movingColour;
        [SerializeField] Color stillColour;
        [SerializeField] float emissionDarkness = 0.5f;
        private GaussianTransform activationFunction;
        // Components that should change colour
        [SerializeField] List<MeshRenderer> colourRenderers;

        [Header("Movement Options")]
        private Animator animationController;
        private bool isAnimating = false;
        // Changes colour depending on whether the lantern is moving or still
        [SerializeField] bool isMoving = false;
        [SerializeField] LanternAnimation currentAnimation;

        // Tracked values
        private Vector3 currentDistance;
        public Vector3 GetCurrentDistance() {  return currentDistance; }

        private float currentActivation;
        public float GetCurrentActivation() { return currentActivation; }

        private void Awake()
        {
            startingPointController = GetComponentInChildren<aStartingPoint>(true);
            lanternLight = GetComponentInChildren<Light>(true);
            activationFunction = GetComponent<GaussianTransform>();
            animationController = GetComponentInChildren<Animator>();

            Debug.Log("Awake: " + gameObject.name);
            isMoving = false;
            isAnimating = false;
            currentAnimation = LanternAnimation.Reset;

            UseStart(false);
            UseChangingColour(false);
        }

        private void Update()
        {
            if (updateColour)
            {
                Vector3 positionDiff = controllerObject.transform.position - lanternCenter.transform.position;
                float positionDiffMag = Math.Abs(positionDiff.magnitude);
                float activationValue = activationFunction.Gaussian(positionDiffMag);

                Color currentColour = isMoving ? movingColour : stillColour;
                Color activatedColour = Color.Lerp(Color.black, currentColour, activationValue);
                Color emissionColour = Color.Lerp(Color.black, activatedColour, emissionDarkness);

                changingMat.color = currentColour;
                changingMat.SetColor("_EmissionColor", emissionColour);

                lanternLight.intensity = maxLightIntensity * activationValue;

                // Save values
                currentDistance = positionDiff;
                currentActivation = activationValue;
            }
            else
            {
                currentDistance = new Vector3(float.NaN, float.NaN, float.NaN);
                currentActivation = float.NaN;
            }
        }

        public void UseStart(bool use)
        {
            startingPointController.EnableStart(use);
        }

        public void UseChangingColour(bool changing)
        {
            Material colourMat = defaultMat;
            if (changing)
            {
                colourMat = changingMat;
            }

            updateColour = changing;
            SetMaterial(colourMat);
        }

        public void SetMaterial(Material newMat)
        {
            foreach (Renderer r in colourRenderers)
            {
                r.material = newMat;
            }
        }

        public void Reset()
        {
            animationController.speed = 1;
        }

        public void TriggerAnimation(LanternAnimation animation)
        {
            animationController.SetTrigger(animation.ToString().ToLower());
            isAnimating = animation == LanternAnimation.Reset ? false : true;
            isMoving = animation == LanternAnimation.Reset ? false : true;

            currentAnimation = animation;
        }

        public void PauseAnimation()
        {
            if (currentAnimation != LanternAnimation.Reset)
            {
                animationController.speed = 0;
                isMoving = false;
            }
        }

        public void PlayAnimation()
        {
            if (currentAnimation != LanternAnimation.Reset)
            {
                animationController.speed = 1;
                isMoving = true;
            }
        }

        internal void FinishCycle()
        {
            Debug.Log("Finish cycle: " + gameObject.name);
            manager.FinishCycle();
        }

        internal void FinishReset()
        {
            // Connected to the "Start" animation state
            // Not used currently, but could be usedful for gradually resetting lantern position at the end of each trial
        }
    } 

    public enum LanternAnimation
    {
        Reset,
        Circle,
        Horizontal,
        Vertical
    }
}