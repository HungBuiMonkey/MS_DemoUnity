using MonkeyBase.Observer;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public struct BERV01FTSInputChanner : EventListener<BERV01FTSInputChanner>
    {
        public BERV01UserInput UserInput;
        public object Data;

        public BERV01FTSInputChanner(BERV01UserInput userInput, object data)
        {
            this.UserInput = userInput;
            this.Data = data;
        }

        public void OnMMEvent(BERV01FTSInputChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}