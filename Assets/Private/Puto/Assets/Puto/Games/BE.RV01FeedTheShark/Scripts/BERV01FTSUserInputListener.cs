using MonkeyBase.Observer;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSUserInputListener : MonoBehaviour, EventListener<BERV01FTSInputChanner>
    {
        public void OnMMEvent(BERV01FTSInputChanner eventType)
        {
            switch (eventType.UserInput)
            {
                case BERV01UserInput.SkipGuiding:
                    BERV01FTSHandleData.TriggerFinishState(BERV01State.PlayGame, (true, true));
                    break;
            }
        }
        private void OnEnable()
        {
            this.ObserverStartListening<BERV01FTSInputChanner>();
        }

        private void OnDisable()
        {
            this.ObserverStopListening<BERV01FTSInputChanner>();
        }
    }
}
