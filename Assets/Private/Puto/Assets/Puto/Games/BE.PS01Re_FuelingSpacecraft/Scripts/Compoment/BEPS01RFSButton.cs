using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSButton : BaseButton
    {
        [SerializeField] BEPS01RFSUserInput userInput;
        public override void Enable(bool isEnable)
        {
            button.interactable = isEnable;
        }

        public override void OnClick()
        {
            if(userInput == BEPS01RFSUserInput.SkipGuiding)
            {
                gameObject.SetActive(false);
                BEPS01RFSHandleData.TriggerStateInput(BEPS01RFSUserInput.SkipGuiding, null);
            }
            else
            {
                BEPS01RFSHandleData.TriggerStateInput(userInput, null);
            }
        }
    }
}
