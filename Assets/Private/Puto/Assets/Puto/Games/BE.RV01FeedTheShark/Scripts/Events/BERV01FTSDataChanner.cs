using MonkeyBase.Observer;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public struct BERV01FTSDataChanner : EventListener<BERV01FTSDataChanner>
    {
        public BERV01State EventName;
        public object Data;

        public BERV01FTSDataChanner(BERV01State nameEvent, object data)
        {
            this.EventName = nameEvent;
            this.Data = data;
        }

        public void OnMMEvent(BERV01FTSDataChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}