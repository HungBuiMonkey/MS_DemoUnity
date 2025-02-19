using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSSystem : FSMSystem
    {
        private FSMState bERV01InitState;
        private FSMState bERV01IntroState;
        private FSMState bERV01PlayState;
        private FSMState bERV01OutroState;


        private void Awake()
        {
            bERV01InitState = new BERV01FTSInitState();
            bERV01IntroState = new BERV01FTSIntroState();
            bERV01PlayState = new BERV01FTSPlayGuidingState();
            bERV01OutroState = new BERV01FTSOutroState();
        }
        public override void GotoState(string eventName, object data)
        {
            BERV01State state = (BERV01State)Enum.Parse(typeof(BERV01State), eventName);
            switch (state)
            {
                case BERV01State.InitData:
                    GotoState(bERV01InitState, data);
                    break;
                case BERV01State.IntroGame:
                    GotoState(bERV01IntroState, data);
                    break;
                case BERV01State.PlayGame:
                    GotoState(bERV01PlayState, data);
                    break;
                case BERV01State.OutroGame:
                    GotoState(bERV01OutroState, data);
                    break;
            }
        }

        public override void SetupStateData<T>(T data)
        {
            if (data is Dependency dependency)
            {
                BERV01FTSInitStateObjectDependency initData = dependency.GetStateData<BERV01FTSInitStateObjectDependency>();
                bERV01InitState.SetUp(initData);

                BERV01FTSIntroStateObjectDependency introData = dependency.GetStateData<BERV01FTSIntroStateObjectDependency>();
                bERV01IntroState.SetUp(introData);

                BERV01FTSPlayStateObjectDependency playData = dependency.GetStateData<BERV01FTSPlayStateObjectDependency>();
                bERV01PlayState.SetUp(playData);

                BERV01FTSOutroStateObjectDependency endGameData = dependency.GetStateData<BERV01FTSOutroStateObjectDependency>();
                bERV01OutroState.SetUp(endGameData);
            }
        }

    }

}