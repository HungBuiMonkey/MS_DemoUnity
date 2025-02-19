using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundSystem : FSMSystem
    {
        private FSMState bEPP01BlendingSoundInitState;
        private FSMState bEPP01BlendingSoundIntroState;
        private FSMState bEPP01BlendingSoundGuidingState;
        private FSMState bEPP01BlendingSoundClickEllieState;
        private FSMState bEPP01BlendingSoundClickUnderlineState;
        private FSMState bEPP01BlendingSoundClickAnswerState;
        private FSMState bEPP01BlendingSoundPrepareNextTurnState;
        private FSMState bEPP01BlendingSoundEndGameState;
        private FSMState bEPP01BlendingSoundNextState;

        private void Awake()
        {
            bEPP01BlendingSoundInitState = new BEPP01BlendingSoundInitState();
            bEPP01BlendingSoundIntroState = new BEPP01BlendingSoundIntroState();
            bEPP01BlendingSoundGuidingState = new BEPP01BlendingSoundGuidingState();
            bEPP01BlendingSoundClickEllieState = new BEPP01BlendingSoundClickEllieState();
            bEPP01BlendingSoundClickUnderlineState = new BEPP01BlendingSoundClickUnderlineState();
            bEPP01BlendingSoundClickAnswerState = new BEPP01BlendingSoundClickAnswerState();
            bEPP01BlendingSoundPrepareNextTurnState = new BEPP01BlendingSoundPrepareNextTurnState();
            bEPP01BlendingSoundEndGameState = new BEPP01BlendingSoundEndGameState();
            bEPP01BlendingSoundNextState = new BEPP01BlendingSoundNextState();
        }

        public override void GotoState(string eventName, object data)
        {

            switch (eventName)
            {
                case BEPP01BlendingSoundEvent.INIT_STATE_START:
                    GotoState(bEPP01BlendingSoundInitState, data);
                    break;
                case BEPP01BlendingSoundEvent.INTRO_STATE_START:
                    GotoState(bEPP01BlendingSoundIntroState, data);
                    break;
                case BEPP01BlendingSoundEvent.GUIDING_STATE:
                    GotoState(bEPP01BlendingSoundGuidingState, data);
                    break;
                case BEPP01BlendingSoundEvent.CLICK_ELLIE_STATE:
                    GotoState(bEPP01BlendingSoundClickEllieState, null);
                    break;
                case BEPP01BlendingSoundEvent.CLICK_UNDERLINE_STATE:
                    GotoState(bEPP01BlendingSoundClickUnderlineState, data);
                    break;
                case BEPP01BlendingSoundEvent.CLICK_ANSWER_STATE:
                    GotoState(bEPP01BlendingSoundClickAnswerState, data);
                    break;
                case BEPP01BlendingSoundEvent.NEXTTURN_STATE_START:
                    GotoState(bEPP01BlendingSoundPrepareNextTurnState, data);
                    break;
                case BEPP01BlendingSoundEvent.ENDGAME_STATE_START:
                    GotoState(bEPP01BlendingSoundEndGameState, data);
                    break;
                case BEPP01BlendingSoundEvent.NEXT_STATE_START:
                    GotoState(bEPP01BlendingSoundNextState, data);
                    break;
            }
        }

        public override void SetupStateData<T>(T data)
        {
            if (data is Dependency dependency)
            {
                BEPP01BlendingSoundInitStateDataObjectDependency initStateData = dependency.GetStateData<BEPP01BlendingSoundInitStateDataObjectDependency>();
                bEPP01BlendingSoundInitState.SetUp(initStateData);
                BEPP01BlendingSoundIntroStateDataObjectDependency introStateData = dependency.GetStateData<BEPP01BlendingSoundIntroStateDataObjectDependency>();
                bEPP01BlendingSoundIntroState.SetUp(introStateData);
                BEPP01BlendingSoundGuidingStateDataObjectDependency guidingStateData = dependency.GetStateData<BEPP01BlendingSoundGuidingStateDataObjectDependency>();
                bEPP01BlendingSoundGuidingState.SetUp(guidingStateData);
                BEPP01BlendingSoundClickEllieStateDataObjectDependency clickEllieStateData = dependency.GetStateData<BEPP01BlendingSoundClickEllieStateDataObjectDependency>();
                bEPP01BlendingSoundClickEllieState.SetUp(clickEllieStateData);
                BEPP01BlendingSoundClickUnderlineStateDataObjectDependency clickUnderlineStateData = dependency.GetStateData<BEPP01BlendingSoundClickUnderlineStateDataObjectDependency>();
                bEPP01BlendingSoundClickUnderlineState.SetUp(clickUnderlineStateData);
                BEPP01BlendingSoundClickAnswerStateDataObjectDependency clickAnswerStateData = dependency.GetStateData<BEPP01BlendingSoundClickAnswerStateDataObjectDependency>();
                bEPP01BlendingSoundClickAnswerState.SetUp(clickAnswerStateData);
                BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency prepareNextTurnStateData = dependency.GetStateData<BEPP01BlendingSoundPrepareNextTurnStateDataObjectDependency>();
                bEPP01BlendingSoundPrepareNextTurnState.SetUp(prepareNextTurnStateData);
                BEPP01BlendingSoundEndGameStateDataObjectDependency endGameStateData = dependency.GetStateData<BEPP01BlendingSoundEndGameStateDataObjectDependency>();
                bEPP01BlendingSoundEndGameState.SetUp(endGameStateData);
                BEPP01BlendingSoundNextStateDependency nextStateDependency = dependency.GetStateData<BEPP01BlendingSoundNextStateDependency>();
                bEPP01BlendingSoundNextState.SetUp(nextStateDependency);
            }
        }
    }
}