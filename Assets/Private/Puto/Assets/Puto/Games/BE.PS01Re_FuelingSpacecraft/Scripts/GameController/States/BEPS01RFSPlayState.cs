using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSPlayState : FSMState
    {
        private BEPS01RFSPlayStateObjectDependency dependency;
        private CancellationTokenSource cts;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPS01RFSPlayStateData playStateData = (BEPS01RFSPlayStateData)data;
            DoWork(playStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPS01RFSPlayStateObjectDependency)data;
        }

        private void DoWork(BEPS01RFSPlayStateData dataPlay)
        {
            cts = new CancellationTokenSource();
            try
            {
                if (dataPlay.DragCorrectCount == dataPlay.MaxTubeDrag)
                {
                    if (dataPlay.TurnGame < dataPlay.MaxTurn)
                    {
                        BEPS01RFSHandleData.TriggerFinishState(BEPS01RFSState.NextTurnGame, null);
                    }
                }else
                {
                    BEPS01RFSHandleData.EnableTubes(dependency.TubeItems, true);
                    if (dataPlay.IsEnableCatSpaceship)
                    {
                        dependency.ButtonCatSpaceship.Enable(true);
                    }
                    dependency.ButtonCatStation.Enable(true);
                    for (int i = 0; i < dependency.TubeItems.Count; i++)
                    {
                        BEPS01RFSTubeItem tubeItem = dependency.TubeItems[i];
                        if (tubeItem.IsDragObject())
                        {
                            BEPS01RFSDashBoxItem dashBoxItem = dependency.DashBoxItems.Find(item => BEPS01RFSHandleData.AreIntegersEqual(tubeItem.GetData().id, item.GetId()));
                            dependency.Guiding.InitData(tubeItem, dashBoxItem);
                            break;
                        }
                    }
                    dependency.Guiding.StartGuiding(true);
                }

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
    public class BEPS01RFSPlayStateData
    {
        public int DragCorrectCount { get; set; }
        public int MaxTubeDrag { get; set; }
        public int TurnGame { get; set; }
        public int MaxTurn { get; set; }
        public bool IsEnableCatSpaceship{ get; set; }
}
  
    public class BEPS01RFSPlayStateObjectDependency
    {
        public BEPS01RFSGuiding Guiding { get; set; }
        public List<BEPS01RFSTubeItem> TubeItems { get; set; }
        public List<BEPS01RFSDashBoxItem> DashBoxItems { get; set; }
        public BaseButton ButtonCatStation { get; set; }
        public BaseButton ButtonCatSpaceship { get; set; }
    }
}