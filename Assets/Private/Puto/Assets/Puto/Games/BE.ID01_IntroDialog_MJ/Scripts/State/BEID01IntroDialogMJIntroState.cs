using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonkeyBase.Observer;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using Spine.Unity;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJIntroState : FSMState
    {
        private BEID01IntroDialogMJIntroStateDependency dependency;
        private CancellationTokenSource cancellationTokenSource;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            cancellationTokenSource = new();
            BEID01IntroDialogMJIntroStateData introStateData = (BEID01IntroDialogMJIntroStateData)data;
            DoWork(introStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEID01IntroDialogMJIntroStateDependency)data;
        }
        private async void DoWork(BEID01IntroDialogMJIntroStateData introStateData)
        {
            try
            {
                dependency.CharatersAnim.AnimationState.SetAnimation(0, dependency.AnimIdle, true);
                var introConfig = dependency.IntroConfig;
                var conversationContainer = dependency.ConversationContainer;
                // max.AnimationState.SetAnimation(0, dependency.AnimNormalToHappy, false);
                //characterController.SetState(CharactersState.Happy);

                conversationContainer.transform.localScale = Vector3.zero;
                conversationContainer.gameObject.SetActive(true);
                await UniTask.Delay(introConfig.milisecondTimeDelay, cancellationToken: cancellationTokenSource.Token);
                bool isPlayingTopic = false;
                SoundChannel topicData;
                
                topicData = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, introConfig.topicSoundMJ, () => { isPlayingTopic = true; });
                ObserverManager.TriggerEvent<SoundChannel>(topicData);
                await UniTask.WaitUntil(() => isPlayingTopic, cancellationToken: cancellationTokenSource.Token);
                
                await UniTask.Delay(introConfig.milisecondTimeDelay, cancellationToken: cancellationTokenSource.Token);
                conversationContainer.transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutBack);

                SoundChannel popupSound = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, introConfig.popupSound);
                ObserverManager.TriggerEvent<SoundChannel>(popupSound);
                await UniTask.Delay(introConfig.milisecondTimeDelay * 10, cancellationToken: cancellationTokenSource.Token);
                await UniTask.Delay(introConfig.milisecondTimeDelay, cancellationToken: cancellationTokenSource.Token);
                BEID01IntroDialogMJEvent bEID01IntroDialogEvent = new BEID01IntroDialogMJEvent(BEID01IntroDialogMJEvent.PLAY_STATE_START, null);
                ObserverManager.TriggerEvent<BEID01IntroDialogMJEvent>(bEID01IntroDialogEvent);
            }
            catch (System.Exception)
            {

            }
        }
        public override void OnExit()
        {
            base.OnExit();
            cancellationTokenSource?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
        }

    }
    public class BEID01IntroDialogMJIntroStateData
    {
        public List<BEID01IntroDialogMJBoxChat> ListBoxChat { get; set; }
    }
    public class BEID01IntroDialogMJIntroStateDependency
    {
        public BEID01MJIntroConfig IntroConfig { get; set; }
        public GameObject CharactersParent { get; set; }
        public BEID01MJConversationContainer ConversationContainer { get; set; }
        public Image BlurPanel { get; set; }
        public SkeletonGraphic CharatersAnim { get; set; }
        public string AnimIdle { get; set; }
        public string AnimTalk { get; set; }

    }
}