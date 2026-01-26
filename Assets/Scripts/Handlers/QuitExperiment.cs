using UnityEngine;

namespace FollowingNoGo
{
    public class QuitExperiment : MonoBehaviour
    {
        [SerializeField] float holdTime;
        private float startHoldTime;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (float.IsNaN(startHoldTime))
                {
                    startHoldTime = Time.time;
                }
                else
                {
                    if (Time.time - startHoldTime > holdTime)
                    {
                        Quit();
                    }
                }
            } else
            {
                startHoldTime = float.NaN;
            }
        }

        private void Quit()
        {
            Debug.Log("THE EXPERIMENT HAS BEEN STOPPED");
#if UNITY_EDITOR_WIN
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}