using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public struct BEPS01RFSDataChanner : EventListener<BEPS01RFSDataChanner>
    {
        public BEPS01RFSState EventName;
        public object Data;

        public BEPS01RFSDataChanner(BEPS01RFSState nameEvent, object data)
        {
            this.EventName = nameEvent;
            this.Data = data;
        }

        public void OnMMEvent(BEPS01RFSDataChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}