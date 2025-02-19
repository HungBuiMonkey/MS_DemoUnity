using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonkeyBase.Observer;
namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public struct BEID01MJUserInputChannel : EventListener<BEID01MJUserInputChannel>
    {
        public string EventName;
        public object Data;
        public BEID01MJUserInputChannel(string nameEvent, object data)
        {
            EventName = nameEvent;
            Data = data;
        }
        public void OnMMEvent(BEID01MJUserInputChannel eventType)
        {

        }
        public const string BUTTON_CLICK = "BUTTON_CLICK";
        public const string BUTTON_REVIEW_CLICK = "BUTTON_REVIEW_CLICK";
        public const string BUTTON_UNCLICK = "BUTTON_UNCLICK";
    }
}