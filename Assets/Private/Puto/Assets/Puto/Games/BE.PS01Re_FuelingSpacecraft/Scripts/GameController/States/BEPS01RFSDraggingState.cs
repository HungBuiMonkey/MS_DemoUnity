using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSDraggingState : FSMState
    {
        private BEPS01RFSDraggingStateObjectDependency dependency;
        private CancellationTokenSource cts;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS01RFSDraggingStateEventData eventData = (BEPS01RFSDraggingStateEventData)data;
            DoWork(eventData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSDraggingStateObjectDependency)data;
        }

        private async void DoWork(BEPS01RFSDraggingStateEventData eventData)
        {
            cts = new CancellationTokenSource();
            try
            {
                dependency.ButtonCatSpaceship.Enable(false);
                dependency.ButtonCatStation.Enable(false);
                dependency.Guiding.ResetGuiding();
               /* BEPS01RFSTubeItem objectDrag = eventData.ObjectEvent.GetComponent<BEPS01RFSTubeItem>();
                BEPS01RFSHandleData.EnableTubes(dependency.TubeItems, objectDrag.GetData().id, false);*/

            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
        }


        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }
    }
    public class BEPS01RFSDraggingStateEventData
    {
        public GameObject ObjectEvent { get; set; }
    }

    public class BEPS01RFSDraggingStateObjectDependency
    {
        public BEPS01RFSGuiding Guiding { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public BaseButton ButtonCatStation { get; set; }
        public BaseButton ButtonCatSpaceship { get; set; }
    }
}