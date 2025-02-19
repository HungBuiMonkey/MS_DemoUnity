using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01Button : MKButton
    {
        public override void OnClick()
        {
            BEPP01BlendingSoundUserInputChanel buttonData = new BEPP01BlendingSoundUserInputChanel(BEPP01BlendingSoundUserInputChanel.BUTTON_CLICK, gameObject);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundUserInputChanel>(buttonData);
        }
    }
}