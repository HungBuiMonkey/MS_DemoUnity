using MonkeyBase.Observer;
using System.Collections;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public struct BEPP01BlendingSoundUserInputChanel : EventListener<BEPP01BlendingSoundUserInputChanel>
    {
        public string EventName;
        public object ObjectInput;
        public void OnMMEvent(BEPP01BlendingSoundUserInputChanel eventType)
        {

        }

        public BEPP01BlendingSoundUserInputChanel(string nameEvent, object objectInput)
        {
            this.EventName = nameEvent;
            this.ObjectInput = objectInput;
        }

        public const string BUTTON_CLICK = "BUTTON_CLICK";

    }
}
