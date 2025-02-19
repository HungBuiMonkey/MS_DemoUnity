using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonkeyBase.Observer;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJUserInputListener : MonoBehaviour, EventListener<BEID01MJUserInputChannel>
    {
        private void OnEnable()
        {
            this.ObserverStartListening<BEID01MJUserInputChannel>();
        }

        private void OnDisable()
        {
            this.ObserverStopListening<BEID01MJUserInputChannel>();
        }
        public void OnMMEvent(BEID01MJUserInputChannel eventType)
        {
            switch (eventType.EventName)
            {
                case BEID01MJUserInputChannel.BUTTON_CLICK:
                    BEID01IntroDialogMJListenStateEventData listenStateEventData = new BEID01IntroDialogMJListenStateEventData();
                    var boxChatQuestion = (GameObject)eventType.Data;
                    listenStateEventData.BoxChat = boxChatQuestion.GetComponent<BEID01IntroDialogMJBoxChat>();
                    BEID01IntroDialogMJEvent bEID01IntroDialogEvent1 = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.LISTEN_STATE_START, listenStateEventData);
                    ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent1);
                    break;

                case BEID01MJUserInputChannel.BUTTON_REVIEW_CLICK:
                    BEID01MJListenAgainStateEventData listenAgainStateEventData = new BEID01MJListenAgainStateEventData();
                    var boxChat = (GameObject)eventType.Data;
                    listenAgainStateEventData.BoxChat = boxChat.GetComponent<BEID01IntroDialogMJBoxChat>();
                    BEID01IntroDialogMJEvent bEID01IntroDialogEvent2 = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.LISTEN_AGAIN_STATE_START, listenAgainStateEventData);
                    ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent2);
                    break;
            }
        }
    }
}