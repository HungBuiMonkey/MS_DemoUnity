using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BEPS02FlyingOwls
{
    public class BEPS02FlyingOwlsSystem : FSMSystem
    {
        private FSMState bEPS02FlyingOwlsInitState;
        private FSMState bEPS02FlyingOwlsIntroState;
        private FSMState bEPS02FlyingOwlsPlayState;
        private FSMState bEPS02FlyingOwlsPlayEffectState;
        private FSMState bEPS02FlyingOwlsClickState;
        private FSMState bEPS02FlyingOwlsDraggingState;
        private FSMState bEPS02FlyingOwlsDragResultState;
        private FSMState bEPS02FlyingOwlsNextTurnState;
        private FSMState bEPS02FlyingOwlsOutroState;

        private void Awake()
        {
            bEPS02FlyingOwlsInitState = new BEPS02FlyingOwlsInitState();
            bEPS02FlyingOwlsIntroState = new BEPS02FlyingOwlsIntroState();
            bEPS02FlyingOwlsPlayState = new BEPS02FlyingOwlsPlayState();
            bEPS02FlyingOwlsPlayEffectState = new BEPS02FlyingOwlsPlayEffectState();
            bEPS02FlyingOwlsClickState = new BEPS02FlyingOwlsClickState();
            bEPS02FlyingOwlsDraggingState = new BEPS02FlyingOwlsDraggingState();
            bEPS02FlyingOwlsDragResultState = new BEPS02FlyingOwlsDragResultState();
            bEPS02FlyingOwlsNextTurnState = new BEPS02FlyingOwlsNextTurnState();
            bEPS02FlyingOwlsOutroState = new BEPS02FlyingOwlsOutroState();
        }

        public override void GotoState(string eventName, object data)
        {
            BEPS02FlyingOwlsState state = (BEPS02FlyingOwlsState)Enum.Parse(typeof(BEPS02FlyingOwlsState), eventName);

            switch (state)
            {
                case BEPS02FlyingOwlsState.InitData:
                    GotoState(bEPS02FlyingOwlsInitState, data);
                    break;
                case BEPS02FlyingOwlsState.IntroGame:
                    GotoState(bEPS02FlyingOwlsIntroState, data);
                    break;
                case BEPS02FlyingOwlsState.PlayGame:
                    GotoState(bEPS02FlyingOwlsPlayState, data);
                    break;
                case BEPS02FlyingOwlsState.PlayEffect:
                    GotoState(bEPS02FlyingOwlsPlayEffectState, data);
                    break;
                case BEPS02FlyingOwlsState.ClickObject:
                    GotoState(bEPS02FlyingOwlsClickState, data);
                    break;
                case BEPS02FlyingOwlsState.DraggingObject:
                    GotoState(bEPS02FlyingOwlsDraggingState, data);
                    break;
                /*case BEPS02FlyingOwlsState.DragResult:
                    GotoState(bEPS02FlyingOwlsDragResultState, data);
                    break;*/
                case BEPS02FlyingOwlsState.NextTurnGame:
                    GotoState(bEPS02FlyingOwlsNextTurnState, data);
                    break;
                case BEPS02FlyingOwlsState.OutroGame:
                    GotoState(bEPS02FlyingOwlsOutroState, data);
                    break;
            }
        }



        public override void SetupStateData<T>(T data)
        {
            if (data is Dependency dependency)
            {
                BEPS02FlyingOwlsInitStateObjectDependency initStateData = dependency.GetStateData<BEPS02FlyingOwlsInitStateObjectDependency>();
                bEPS02FlyingOwlsInitState.SetUp(initStateData);

                BEPS02FlyingOwlsIntroStateObjectDependency introStateData = dependency.GetStateData<BEPS02FlyingOwlsIntroStateObjectDependency>();
                bEPS02FlyingOwlsIntroState.SetUp(introStateData);

                BEPS02FlyingOwlsPlayStateObjectDependency playStateData = dependency.GetStateData<BEPS02FlyingOwlsPlayStateObjectDependency>();
                bEPS02FlyingOwlsPlayState.SetUp(playStateData);

                BEPS02FlyingOwlsPlayEffectStateObjectDependency playEffectStateData = dependency.GetStateData<BEPS02FlyingOwlsPlayEffectStateObjectDependency>();
                bEPS02FlyingOwlsPlayEffectState.SetUp(playEffectStateData);

                BEPS02FlyingOwlsClickStateObjectDependency clickStateData = dependency.GetStateData<BEPS02FlyingOwlsClickStateObjectDependency>();
                bEPS02FlyingOwlsClickState.SetUp(clickStateData);

                BEPS02FlyingOwlsDraggingStateObjectDependency draggingStateData = dependency.GetStateData<BEPS02FlyingOwlsDraggingStateObjectDependency>();
                bEPS02FlyingOwlsDraggingState.SetUp(draggingStateData);

                /*BEPS02FlyingOwlsDragResultStateObjectDependency dragResultStateData = dependency.GetStateData<BEPS02FlyingOwlsDragResultStateObjectDependency>();
                bEPS02FlyingOwlsDragResultState.SetUp(dragResultStateData);*/

                BEPS02FlyingOwlsNextTurnStateObjectDependency nextTurnStateData = dependency.GetStateData<BEPS02FlyingOwlsNextTurnStateObjectDependency>();
                bEPS02FlyingOwlsNextTurnState.SetUp(nextTurnStateData);

                BEPS02FlyingOwlsOutroStateObjectDependency outroStateData = dependency.GetStateData<BEPS02FlyingOwlsOutroStateObjectDependency>();
                bEPS02FlyingOwlsOutroState.SetUp(outroStateData);
            }
        }

    }
}