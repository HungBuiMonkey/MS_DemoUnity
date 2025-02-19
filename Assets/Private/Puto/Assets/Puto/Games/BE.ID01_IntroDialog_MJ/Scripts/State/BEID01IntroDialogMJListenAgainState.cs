using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonkeyBase.Observer;
using System;
using Spine.Unity;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJListenAgainState : FSMState
    {
        private BEID01IntroDialogMJListenAgainStateDependency dependency;
        private const string BOX_CHAT_ANSWER = "BoxChatAnswerMJ";
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEID01IntroDialogMJListenAgainStateData listenAgainStateData = (BEID01IntroDialogMJListenAgainStateData)data;
            DoWork(listenAgainStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEID01IntroDialogMJListenAgainStateDependency)data;
        }
        private void DoWork(BEID01IntroDialogMJListenAgainStateData listenAgainStateData)
        {
            SoundManager.Instance.StopFxOneShot();
            var listBoxChat = listenAgainStateData.ListBoxChat;
            foreach (var item in listBoxChat)
            {
                item.SetReviewClick(false);
            }
            var boxChat = listenAgainStateData.EventData.BoxChat;
            var audioClip = boxChat.GetAudio();
            Action actionDone = () =>
            {
            //max.AnimationState.SetAnimation(0, dependency.AnimNormalToHappy, false);
            BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.REVIEW_STATE_START, null);
                ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
            };
            // max.AnimationState.SetAnimation(0, dependency.AnimTalk, true);
           
            SoundChannel audio = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, audioClip, actionDone);
            ObserverManager.TriggerEvent<SoundChannel>(audio);

        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
    public class BEID01IntroDialogMJListenAgainStateData
    {
        public BEID01MJListenAgainStateEventData EventData { get; set; }

        public List<BEID01IntroDialogMJBoxChat> ListBoxChat { get; set; }
    }
    public class BEID01MJListenAgainStateEventData
    {
        public BEID01IntroDialogMJBoxChat BoxChat { get; set; }
    }
    public class BEID01IntroDialogMJListenAgainStateDependency
    {
        public GameObject CharactersParent { get; set; }
        public SkeletonGraphic CharatersAnim { get; set; }

    }
}