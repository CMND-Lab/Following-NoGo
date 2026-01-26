using UnityEngine;

namespace FollowingNoGo
{
    public class PreSession : MonoBehaviour
    {
        public GameObject canvas;
        public GameObject experiment;

        private void Awake()
        {
            canvas.SetActive(false);
            experiment.SetActive(false);
        }
    }
}

