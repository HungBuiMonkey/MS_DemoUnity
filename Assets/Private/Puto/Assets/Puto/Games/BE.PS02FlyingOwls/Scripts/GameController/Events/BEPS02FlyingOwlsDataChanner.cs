using MonkeyBase.Observer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public struct BEPS02FlyingOwlsDataChanner : EventListener<BEPS02FlyingOwlsDataChanner>
    {
        public BEPS02FlyingOwlsState EventName;
        public object Data;

        public BEPS02FlyingOwlsDataChanner(BEPS02FlyingOwlsState nameEvent, object data)
        {
            this.EventName = nameEvent;
            this.Data = data;
        }

        public void OnMMEvent(BEPS02FlyingOwlsDataChanner eventType)
        {
            throw new System.NotImplementedException();
        }
    }
}