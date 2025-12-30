using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SensorimotorContingencies
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

