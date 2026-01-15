using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXF;
namespace FollowingNoGo
{
    public class GenerateExperiment : MonoBehaviour
    {
        [Header("Baseline trial")]
        public float baselineTrialLength;

        [Header("Experimental Trial Settings")]
        public int numBlocks;
        public int numTrialsPerBlock;

        public float minStopTime = 2;
        public float maxStopTime = 5;
        public float endTrialDelay = 2.5f;

        [Header("Double-Stop Settings")]
        public List<float> doubleStopDelayIntervals = new List<float>();

        public void Generate(Session session)
        {
            //*** BASELINE TRIALS ***//
            Block baselineBlock = session.CreateBlock();
            Trial baselineTrial = baselineBlock.CreateTrial();
            TrialSetting baselineSettings = new TrialSetting(baselineTrialLength);
            baselineTrial.settings.SetValue("settings", baselineSettings);
            baselineBlock.settings.SetValue("type", TrialType.Baseline);

            //*** EXPERIMENTAL BLOCKS ***//
            // Seperate words according to number of blocks
            session.settings.SetValue("n_experimental_blocks", numBlocks);
            session.settings.SetValue("n_trials_per_block", numTrialsPerBlock);

            Block[] experimentalBlocks = new Block[numBlocks];
            for (int blockIndex = 0; blockIndex < numBlocks; blockIndex++)
            {
                Block newBlock = session.CreateBlock();
                newBlock.settings.SetValue("type", TrialType.Experiment);

                float randomStopTime;
                List<StopEvent> trialEvents;
                while (newBlock.trials.Count < numTrialsPerBlock)
                {
                    // 2 double stop simultaneous

                    randomStopTime = RandomStopTime();
                    trialEvents = new List<StopEvent>
                    { 
                        new StopEvent(randomStopTime, LanternLocaction.Both) 
                    };
                    MakeTrial(newBlock, randomStopTime, trialEvents);

                    randomStopTime = RandomStopTime();
                    trialEvents = new List<StopEvent> 
                    { 
                        new StopEvent(randomStopTime, LanternLocaction.Both) 
                    };
                    MakeTrial(newBlock, randomStopTime, trialEvents);

                    // 2 double stop delayed for each delay interval
                    foreach (float delay in doubleStopDelayIntervals)
                    {
                        // Left stops first
                        randomStopTime = RandomStopTime();
                        trialEvents = new List<StopEvent> 
                        { 
                            new StopEvent(randomStopTime, LanternLocaction.Left),
                            new StopEvent(delay, LanternLocaction.Right)
                        };
                        MakeTrial(newBlock, randomStopTime, trialEvents);

                        // Right stops first
                        randomStopTime = RandomStopTime();
                        trialEvents = new List<StopEvent>
                        {
                            new StopEvent(randomStopTime, LanternLocaction.Right),
                            new StopEvent(delay, LanternLocaction.Left)
                        };
                        MakeTrial(newBlock, randomStopTime, trialEvents);
                    }

                    // 2 single-stop
                    randomStopTime = RandomStopTime();
                    trialEvents = new List<StopEvent>
                    {
                        new StopEvent(randomStopTime, LanternLocaction.Left)
                    };
                    MakeTrial(newBlock, randomStopTime, trialEvents);

                    randomStopTime = RandomStopTime();
                    trialEvents = new List<StopEvent>
                    {
                        new StopEvent(randomStopTime, LanternLocaction.Right)
                    };
                    MakeTrial(newBlock, randomStopTime, trialEvents);
                }

                newBlock.trials.Shuffle();
            }
        }

        public Trial MakeTrial(Block block, float duration, List<StopEvent> events)
        {
            TrialSetting trialSetting = new TrialSetting(duration + endTrialDelay, events);
            Trial newTrial = block.CreateTrial();
            newTrial.settings.SetValue("settings", trialSetting);

            return newTrial;
        }

        public float RandomStopTime()
        {
            return Random.Range(minStopTime, maxStopTime);
        }

        public void ShuffleList<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }
    }

    public enum TrialType { Baseline, Practice, Experiment }
}

