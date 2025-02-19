using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS01Re_FuelingSpacecraft
{
    public class BEPS01RFSSystem : FSMSystem
    {
        private FSMState bEPS01RFSInitState;
        private FSMState bEPS01RFSIntroState;
        private FSMState bEPS01RFSPlayState;
        private FSMState bEPS01RFSClickObjectState;
        private FSMState bEPS01RFSDraggingState;
        private FSMState bEPS01RFSDragResultState;
        private FSMState bEPS01RFSNextTurnState;
        private FSMState bEPS01RFSEndGameState;


        private void Awake()
        {
            bEPS01RFSInitState = new BEPS01RFSInitState();
            bEPS01RFSIntroState = new BEPS01RFSIntroState();
            bEPS01RFSPlayState = new BEPS01RFSPlayState();
            bEPS01RFSClickObjectState = new BEPS01RFSClickState();
            bEPS01RFSDraggingState = new BEPS01RFSDraggingState();
            bEPS01RFSDragResultState = new BEPS01RFSDragResultState();
            bEPS01RFSNextTurnState = new BEPS01RFSNextTurnState();
            bEPS01RFSEndGameState = new BEPS01RFSEndGameState();
        }

        public override void GotoState(string eventName, object data)
        {
            BEPS01RFSState state = (BEPS01RFSState)Enum.Parse(typeof(BEPS01RFSState), eventName);
            switch (state)
            {
                case BEPS01RFSState.InitData:
                    GotoState(bEPS01RFSInitState, data);
                    break;
                case BEPS01RFSState.IntroGame:
                    GotoState(bEPS01RFSIntroState, data);
                    break;
                case BEPS01RFSState.PlayGame:
                    GotoState(bEPS01RFSPlayState, data);
                    break;
                case BEPS01RFSState.ClickObject:
                    GotoState(bEPS01RFSClickObjectState, data);
                    break;
                case BEPS01RFSState.DraggingObject:
                    GotoState(bEPS01RFSDraggingState, data);
                    break;
                /*case BEPS01RFSState.DragResult:
                    GotoState(bEPS01RFSDragResultState, data);
                    break;*/
                case BEPS01RFSState.NextTurnGame:
                    GotoState(bEPS01RFSNextTurnState, data);
                    break;
                case BEPS01RFSState.EndGame:
                    GotoState(bEPS01RFSEndGameState, data);
                    break;
            }
        }
        public override void SetupStateData<T>(T data)
        {
            if (data is Dependency dependency)
            {
                BEPS01RFSInitStateObjectDependency initData = dependency.GetStateData<BEPS01RFSInitStateObjectDependency>();
                bEPS01RFSInitState.SetUp(initData);

                BEPS01RFSIntroStateObjectDependency introData = dependency.GetStateData<BEPS01RFSIntroStateObjectDependency>();
                bEPS01RFSIntroState.SetUp(introData);

                BEPS01RFSPlayStateObjectDependency playData = dependency.GetStateData<BEPS01RFSPlayStateObjectDependency>();
                bEPS01RFSPlayState.SetUp(playData);

                BEPS01RFSClickStateObjectDependency clickData = dependency.GetStateData<BEPS01RFSClickStateObjectDependency>();
                bEPS01RFSClickObjectState.SetUp(clickData);

                BEPS01RFSDraggingStateObjectDependency draggingData = dependency.GetStateData<BEPS01RFSDraggingStateObjectDependency>();
                bEPS01RFSDraggingState.SetUp(draggingData);

                /*BEPS01RFSDragResultStateObjectDependency dragResultData = dependency.GetStateData<BEPS01RFSDragResultStateObjectDependency>();
                bEPS01RFSDragResultState.SetUp(dragResultData);*/

                BEPS01RFSNextTurnStateObjectDependency nextTurnData = dependency.GetStateData<BEPS01RFSNextTurnStateObjectDependency>();
                bEPS01RFSNextTurnState.SetUp(nextTurnData);

                BEPS01RFSEndGameStateObjectDependency endGameData = dependency.GetStateData<BEPS01RFSEndGameStateObjectDependency>();
                bEPS01RFSEndGameState.SetUp(endGameData);
            }
        }
    }
}