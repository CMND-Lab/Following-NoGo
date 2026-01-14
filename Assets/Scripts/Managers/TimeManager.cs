using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class TimeManager : MonoBehaviour
    {
        public TaskController taskController;
        public LanternManager lanternManager;

        private Coroutine countdown;
        private Coroutine lanternEvent;

        [SerializeField] List<StopEvent> events;
        [SerializeField] float trialDuration = 2.5f;

        public void SetTrial(TrialSetting settings)
        {
            trialDuration = settings.GetDuration();
            events = settings.GetEvents();

            Debug.Log(settings.ToString());
        }

        public void BeginCountdown()
        {
            countdown = StartCoroutine(Countdown());
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
            yield return new WaitForSeconds(trialDuration);

            taskController.TimerEnd();
        }

        public void RunEvents()
        {
            if (events != null & events.Count > 0) 
            { 
                foreach (StopEvent e in events)
                {
                    lanternEvent = StartCoroutine(StartEvent(e));
                }
            }
        }

        IEnumerator StartEvent(StopEvent e)
        {
            yield return new WaitForSeconds(e.GetDelay());
            lanternManager.PauseAnimation(e.GetTarget());
        }
    }
}


