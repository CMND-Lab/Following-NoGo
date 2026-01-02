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

        // Tracked values
        private float currentDistance;
        private float currentActivation;

        private void Awake()
        {
            startingPointController = GetComponentInChildren<aStartingPoint>(true);
            UseStart(false);

            lanternLight = GetComponentInChildren<Light>(true);
            activationFunction = GetComponent<GaussianTransform>();
            animationController = GetComponent<Animator>();

            UseChangingColour(false);
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

        private void Update()
        {
            if (updateColour) {
                Vector3 positionDiff = controllerObject.transform.position - lanternCenter.transform.position;
                float positionDiffMag = Math.Abs(positionDiff.magnitude);

                Debug.Log("PosDiff:" + positionDiffMag);

                float activationValue = activationFunction.Gaussian(positionDiffMag);
                Debug.Log("Activation:" + activationValue);

                Color newColour = Color.Lerp(Color.black, isMoving ? movingColour : stillColour, activationValue);
                // changingMat.color = newColour;

                Color emissionColour = Color.Lerp(Color.black, newColour, emissionDarkness);
                changingMat.SetColor("_EmissionColor", emissionColour);

                lanternLight.intensity = maxLightIntensity * activationValue;

                // Save values
                currentDistance = positionDiffMag;
                currentActivation = activationValue;
            } else
            {
                currentDistance = float.NaN;
                currentActivation = float.NaN;
            }
        }
    } 

}