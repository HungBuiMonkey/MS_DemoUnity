using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonkeyBase.Observer;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJGameManager : GameManager, EventListener<BEID01IntroDialogMJEvent>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            this.ObserverStartListening<BEID01IntroDialogMJEvent>();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            this.ObserverStopListening<BEID01IntroDialogMJEvent>();
        }
        public override void SetData<T>(T data)
        {
            base.SetData(data);
            adapter.SetData(data);
        }
        protected override void Start()
        {
            base.Start();
            fSMSystem.SetupStateData(dependency);

            BEID01IntroDialogMJInitStateData initStateData = new BEID01IntroDialogMJInitStateData();
            BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.INIT_STATE_START, initStateData);
            ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
        }
        public void OnMMEvent(BEID01IntroDialogMJEvent eventType)
        {
            (string eventName, object data) navigatorData = navigator.GetData(adapter, eventType.EventName, eventType.Data);
            fSMSystem.GotoState(navigatorData.eventName, navigatorData.data);
        }
    }
}