using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsManager : GameManager, EventListener<BEPS02FlyingOwlsDataChanner>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            this.ObserverStartListening<BEPS02FlyingOwlsDataChanner>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.ObserverStopListening<BEPS02FlyingOwlsDataChanner>();
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
            BEPS02FlyingOwlsDataChanner bEPS02FlyingOwlsEvent = new BEPS02FlyingOwlsDataChanner(BEPS02FlyingOwlsState.InitData, null);
            ObserverManager.TriggerEvent<BEPS02FlyingOwlsDataChanner>(bEPS02FlyingOwlsEvent);
        }

        public void OnMMEvent(BEPS02FlyingOwlsDataChanner eventType)
        {
            (string eventName, object data) navigatorData = navigator.GetData(adapter, eventType.EventName.ToString(), eventType.Data); ;
            fSMSystem.GotoState(navigatorData.eventName, navigatorData.data);
        }

    }
}