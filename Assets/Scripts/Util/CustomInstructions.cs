using UnityEngine;
using UXF;

namespace FollowingNoGo
{
    public class CustomInstructions : MonoBehaviour
    {
        [SerializeField] string replaceValue = "%";
        [SerializeField] string[] settingKeys;
        void Awake()
        {
            InstructionController instruction = GetComponent<InstructionController>();
            if (settingKeys.Length > 0 && instruction != null)
            {
                string instructionText = instruction.GetText();

                foreach (string key in settingKeys)
                {
                    string settingValue = Session.instance.settings.GetObject(key).ToString();
                    Debug.Log("Custom instruction from settings:\t" + key + " : " + settingValue);

                    int pos = instructionText.IndexOf(replaceValue);
                    if (pos >= 0)
                    {
                        instructionText = instructionText.Substring(0, pos) + settingValue + instructionText.Substring(pos + replaceValue.Length);
                    }
                }

                Debug.Log(instructionText);
                instruction.SetInstructionText(instructionText);
            }
        }
    }
}