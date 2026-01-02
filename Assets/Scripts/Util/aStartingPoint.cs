using System.Collections;
using System.Drawing.Printing;
using TMPro.EditorUtilities;
using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public abstract class aStartingPoint: MonoBehaviour
    {
        [SerializeField] protected Material responseOrbMatDim;
        [SerializeField] protected Material responseOrbMatLight;
        [SerializeField] LanternController lanternController;

        protected Collider orbCollider;
        protected MeshRenderer orbRenderer;

        protected StartingStateVR state = StartingStateVR.Waiting;
        protected Coroutine cueCoroutine;


        [Header("Usage")]
        public LanternLocaction location;
        public LanternManager lanternManager;
        public TaskController taskController;
        public CanvasController canvasController;

        //[SerializeField] protected bool hideAfterStartTrial = true;

        [SerializeField] protected float preHoldTime = 0.5f;
        [SerializeField] protected float holdTime = 1.0f;

        public void EnableStart(bool enable)
        {
            gameObject.SetActive(enable);
            if (enable)
            {
                lanternController.SetMaterial(responseOrbMatDim);
            }
        }

        private Collider OrbCollider()
        {
            if (orbCollider == null) { orbCollider = GetComponent<Collider>(); }
            return orbCollider;
        }

        private MeshRenderer OrbRenderer()
        {
            if (orbRenderer == null) { orbRenderer = GetComponent<MeshRenderer>(); }
            return orbRenderer;
        }

        void OnTriggerEnter(Collider other)
        {
            Debug.Log("Holding in " + gameObject.name);
            if (other.CompareTag("GameController"))
            {
                lanternController.SetMaterial(responseOrbMatLight);
                switch (state)
                {
                    case StartingStateVR.Waiting:
                        cueCoroutine = StartCoroutine(HoldingSequence());
                        break;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            Debug.Log("Exited " + gameObject.name);
            if (other.CompareTag("GameController"))
            {
                lanternController.SetMaterial(responseOrbMatDim);
                lanternManager.ExitLantern(location);

                switch (state)
                {
                    case StartingStateVR.GetReady:
                        StopCoroutine(cueCoroutine);
                        //ResetState();
                        break;

                    case StartingStateVR.Go:
                        //ToggleCollider(false);
                        //ToggleRenderer(false);
                        state = StartingStateVR.Waiting;
                        break;
                }
            }
        }

        /**
        public void LightOn()
        {
            OrbRenderer().material = responseOrbMatLight;
        }
        public void LightOff()
        {
            OrbRenderer().material = responseOrbMat;
        }**/

        public void ToggleCollider(bool active)
        {
            OrbCollider().enabled = active;
        }

        public void ToggleRenderer(bool active)
        {
            OrbRenderer().enabled = active;
        }

        public void ResetState()
        {
            state = StartingStateVR.Waiting;
            //LightOff();
            ToggleCollider(true);
            ToggleRenderer(true);
        }

        public void ShowOrb()
        {
            gameObject.SetActive(true);
            ToggleCollider(false);
            ToggleRenderer(true);
        }

        public void Disappear()
        {
            ToggleCollider(false);
            ToggleRenderer(false);
        }

        protected abstract IEnumerator HoldingSequence();
    }

    public enum StartingStateVR
    {
        Waiting,
        GetReady,
        Go
    }
}