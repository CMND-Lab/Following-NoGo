using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class TimeManager : MonoBehaviour
    {
        public TaskController taskController;

        private Coroutine countdown;

        public float timerCount = 2.5f;

        public void SetDuration(float duration)
        {
            timerCount = duration;
        }

        public void BeginCountdown()
        {
            //if (session.CurrentTrial.number > session.settings.GetInt("n_baseline_trials"))
            //{
                countdown = StartCoroutine(Countdown());
            //}
        }

        public void StopCountdown()
        {
            if (countdown != null) {
                StopCoroutine(countdown);
            }
            countdown = null;
        }

        IEnumerator Countdown()
        {
            yield return new WaitForSeconds(timerCount);

            taskController.TimerEnd();
        }
    }
}


