using MonkeyBase.Observer;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSManager : GameManager, EventListener<BERV01FTSDataChanner>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            this.ObserverStartListening<BERV01FTSDataChanner>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            this.ObserverStopListening<BERV01FTSDataChanner>();
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
            BERV01FTSDataChanner dataChanner = new BERV01FTSDataChanner(BERV01State.InitData, null);
            ObserverManager.TriggerEvent(dataChanner);
        }

        public void OnMMEvent(BERV01FTSDataChanner eventType)
        {
            (string eventName, object data) navigatorData = navigator.GetData(adapter, eventType.EventName.ToString(), eventType.Data); ;
            fSMSystem.GotoState(navigatorData.eventName, navigatorData.data);
        }
    }
}