using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJSystem : FSMSystem
    {
        private FSMState initState;
        private FSMState introState;
        private FSMState playState;
        private FSMState reviewState;
        private FSMState listenAgainState;
        private FSMState listenState;
        private void Awake()
        {
            initState = new BEID01IntroDialogMJInitState();
            introState = new BEID01IntroDialogMJIntroState();
            playState = new BEID01IntroDialogMJPlayState();
            reviewState = new BEID01IntroDialogMJReviewState();
            listenState = new BEID01IntroDialogMJListenState();
            listenAgainState = new BEID01IntroDialogMJListenAgainState();
        }
        public override void GotoState(string eventName, object data)
        {
            switch (eventName)
            {
                case BEID01IntroDialogMJEvent.INIT_STATE_START:
                    GotoState(initState, data);
                    break;

                case BEID01IntroDialogMJEvent.INTRO_STATE_START:
                    GotoState(introState, data);
                    break;

                case BEID01IntroDialogMJEvent.PLAY_STATE_START:
                    GotoState(playState, data);
                    break;

                case BEID01IntroDialogMJEvent.LISTEN_AGAIN_STATE_START:
                    GotoState(listenAgainState, data);
                    break;

                case BEID01IntroDialogMJEvent.REVIEW_STATE_START:
                    GotoState(reviewState, data);
                    break;

                case BEID01IntroDialogMJEvent.LISTEN_STATE_START:
                    GotoState(listenState, data);
                    break;
            }

        }
        public override void SetupStateData<T>(T data)
        {
            if (data is Dependency dependency)
            {
                BEID01IntroDialogMJInitStateDependency initStateDependency = dependency.GetStateData<BEID01IntroDialogMJInitStateDependency>();
                initState.SetUp(initStateDependency);

                BEID01IntroDialogMJIntroStateDependency introStateDependency = dependency.GetStateData<BEID01IntroDialogMJIntroStateDependency>();
                introState.SetUp(introStateDependency);

                BEID01IntroDialogMJPlayStateDependency playStateDependency = dependency.GetStateData<BEID01IntroDialogMJPlayStateDependency>();
                playState.SetUp(playStateDependency);

                BEID01IntroDialogMJListenStateDependency listenStateDependency = dependency.GetStateData<BEID01IntroDialogMJListenStateDependency>();
                listenState.SetUp(listenStateDependency);

                BEID01IntroDialogMJListenAgainStateDependency listenAgainStateDependency = dependency.GetStateData<BEID01IntroDialogMJListenAgainStateDependency>();
                listenAgainState.SetUp(listenAgainStateDependency);

                BEID01IntroDialogMJReviewStateDependency reviewStateDependency = dependency.GetStateData<BEID01IntroDialogMJReviewStateDependency>();
                reviewState.SetUp(reviewStateDependency);
            }
        }
    }
}