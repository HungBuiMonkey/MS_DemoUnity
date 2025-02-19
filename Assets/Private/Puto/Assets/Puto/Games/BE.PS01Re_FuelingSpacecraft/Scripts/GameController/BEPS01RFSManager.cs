using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSManager : GameManager, EventListener<BEPS01RFSDataChanner>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            this.ObserverStartListening<BEPS01RFSDataChanner>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.ObserverStopListening<BEPS01RFSDataChanner>();
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
            BEPS01RFSDataChanner dataChanner = new BEPS01RFSDataChanner(BEPS01RFSState.InitData, null);
            ObserverManager.TriggerEvent(dataChanner);
        }

        public void OnMMEvent(BEPS01RFSDataChanner eventType)
        {
            (string eventName, object data) navigatorData = navigator.GetData(adapter, eventType.EventName.ToString(), eventType.Data); ;
            fSMSystem.GotoState(navigatorData.eventName, navigatorData.data);
        }
    }
}