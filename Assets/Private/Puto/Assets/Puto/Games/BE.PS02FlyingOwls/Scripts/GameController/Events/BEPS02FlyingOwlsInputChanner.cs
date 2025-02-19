using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public struct BEPS02FlyingOwlsInputChanner : EventListener<BEPS02FlyingOwlsInputChanner>
    {
        public BEPS02FlyingOwlsUserInput UserInput;
        public object Data;

        public BEPS02FlyingOwlsInputChanner(BEPS02FlyingOwlsUserInput userInput, object data)
        {
            this.UserInput = userInput;
            this.Data = data;
        }

        public void OnMMEvent(BEPS02FlyingOwlsInputChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}