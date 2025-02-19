using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSButton : BaseButton
    {
        [SerializeField] BERV01UserInput userInput;
        public override void Enable(bool isEnable)
        {
            button.interactable = isEnable;
        }

        public override void OnClick()
        {
            BERV01FTSHandleData.TriggerStateInput(userInput, null);
        }
    }
}
