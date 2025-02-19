using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundGameManager : GameManager, EventListener<BEPP01BlendingSoundEvent>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            this.ObserverStartListening<BEPP01BlendingSoundEvent>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.ObserverStopListening<BEPP01BlendingSoundEvent>();
        }

        protected override void Start()
        {
            base.Start();
            fSMSystem.SetupStateData(dependency);

            BEPP01BlendingSoundInitStateData initStateData = new BEPP01BlendingSoundInitStateData();
            BEPP01BlendingSoundEvent bEPD01BlendingSoundEvent = new BEPP01BlendingSoundEvent(BEPP01BlendingSoundEvent.INIT_STATE_START, initStateData);
            ObserverManager.TriggerEvent<BEPP01BlendingSoundEvent>(bEPD01BlendingSoundEvent);
        }

        public void OnMMEvent(BEPP01BlendingSoundEvent eventType)
        {
            (string eventName, object data) navigatorData = navigator.GetData(adapter, eventType.EventName, eventType.Data);

            fSMSystem.GotoState(navigatorData.eventName, navigatorData.data);
        }
        public override void SetData<T>(T data)
        {
            base.SetData(data);
            adapter.SetData<T>(data);
        }
    }
}