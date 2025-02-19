using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public struct BERV01FTSFishChanner : EventListener<BERV01FTSFishChanner>
    {
        public BERV01FishState UserInput;
        public object Data;

        public BERV01FTSFishChanner(BERV01FishState userInput, object data)
        {
            this.UserInput = userInput;
            this.Data = data;
        }

        public void OnMMEvent(BERV01FTSFishChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}