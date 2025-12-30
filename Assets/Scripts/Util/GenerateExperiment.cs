using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UXF;
namespace SensorimotorContingencies
{
    public class GenerateExperiment : MonoBehaviour
    {
        public List<List<GameObject>> blocks;

        public void Generate(Session session)
        {
            // Retrieve blocks
            BlockSetting[] blockSettings = GetComponentsInChildren<BlockSetting>(true);
            int numberOfBlocks = blockSettings.Length;
            Debug.Log("Creating " + numberOfBlocks + " blocks");

            session.settings.SetValue("n_experimental_blocks", numberOfBlocks);

            //*** EXPERIMENTAL BLOCKS ***//
            // Seperate words according to number of blocks
            Block[] experimentalBlocks = new Block[numberOfBlocks];
            for (int blockIndex = 0; blockIndex < numberOfBlocks; blockIndex++)
            {
                // Get block settings from GameObject
                BlockSetting blockSetting = blockSettings[blockIndex];
                List<TrialSetting> blockTrials = blockSetting.GetTrialList();
                
                if (blockSetting.randomiseTrialOrder)
                {
                    // Shuffle list in place
                    ShuffleList(blockTrials);
                }

                int numTrialsInBlock = blockTrials.Count;

                Debug.Log("Block " + (blockIndex + 1) + " : Creating " + numTrialsInBlock + " trials");

                Block newBlock = new Block((uint)numTrialsInBlock, session);
                newBlock.settings.SetValue("type", TrialType.Experiment);

                // Assign settings for each trial
                for (int trialIndex = 0; trialIndex < numTrialsInBlock; trialIndex++)
                {
                    newBlock.GetRelativeTrial(trialIndex + 1).settings.SetValue("settings", blockTrials[trialIndex]);
                }

                experimentalBlocks[blockIndex] = newBlock;
            }
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

