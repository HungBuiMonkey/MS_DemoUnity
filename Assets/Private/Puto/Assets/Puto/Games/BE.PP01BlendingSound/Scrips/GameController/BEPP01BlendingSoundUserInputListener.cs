using MonkeyBase.Observer;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundUserInputListener : MonoBehaviour, EventListener<BEPP01BlendingSoundUserInputChanel>
    {
        private const string ANSWER = "Answer";
        private const string UNDERLINE = "Underline";
        public void OnMMEvent(BEPP01BlendingSoundUserInputChanel eventType)
        {
            GameObject objectClick = (GameObject)eventType.ObjectInput;
            switch (eventType.EventName)
            {
                case BEPP01BlendingSoundUserInputChanel.BUTTON_CLICK:
                    if (objectClick.name.Contains("Ellie"))
                    {
                        BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.CLICK_ELLIE_STATE, null);
                        ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
                    }
                    else if (objectClick.name.Contains(UNDERLINE))
                    {
                        BEPP01BlendingSoundClickUnderlineStateEventData clickUnderlineStateEventData = new();
                        clickUnderlineStateEventData.underlineClick = objectClick.GetComponent<BEPP01Text>();
                        BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.CLICK_UNDERLINE_STATE, clickUnderlineStateEventData);
                        ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
                    }
                    else if (objectClick.name.Contains(ANSWER))
                    {
                        string strAnswerClick = objectClick.name.Substring(ANSWER.Length);
                        BEPP01BlendingSoundClickAnswerStateEventData clickAnswerStateEventData = new();
                        clickAnswerStateEventData.OClick = objectClick;
                        clickAnswerStateEventData.strAnswerClick = strAnswerClick;
                        clickAnswerStateEventData.indexesAnswerClick = objectClick.GetComponentInChildren<BEPP01Text>().GetIndexes();
                        BEPP01BlendingSoundEvent bEPP01BlendingSoundEvent = new(BEPP01BlendingSoundEvent.CLICK_ANSWER_STATE, clickAnswerStateEventData);
                        ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPP01BlendingSoundEvent);
                    }
                    break;
            }
        }
        private void OnEnable()
        {
            this.ObserverStartListening<BEPP01BlendingSoundUserInputChanel>();
        }

        private void OnDisable()
        {
            this.ObserverStopListening<BEPP01BlendingSoundUserInputChanel>();
        }
    }
}