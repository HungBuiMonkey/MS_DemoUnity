using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public struct BEPS01RFSInputChanner : EventListener<BEPS01RFSInputChanner>
    {
        public BEPS01RFSUserInput UserInput;
        public object Data;

        public BEPS01RFSInputChanner(BEPS01RFSUserInput userInput, object data)
        {
            this.UserInput = userInput;
            this.Data = data;
        }

        public void OnMMEvent(BEPS01RFSInputChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}