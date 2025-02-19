using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonkeyBase.Observer;
using Cysharp.Threading.Tasks;
using System.Threading;
using Spine.Unity;

namespace Monkey.MonkeyGo.BEID01IntroDialogMJ
{
    public class BEID01IntroDialogMJListenState : FSMState
    {
        private BEID01IntroDialogMJListenStateDependency dependency;
        private CancellationTokenSource cancellationTokenSource;
        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            cancellationTokenSource = new();
            BEID01IntroDialogMJListenStateData listenStateData = (BEID01IntroDialogMJListenStateData)data;
            DoWork(listenStateData);
        }
        public override void SetUp(object data)
        {
            dependency = (BEID01IntroDialogMJListenStateDependency)data;
        }
        private async void DoWork(BEID01IntroDialogMJListenStateData listenStateData)
        {
            try
            {
                //var max = dependency.Max;
                var boxChat = listenStateData.EventData.BoxChat.GetComponent<BEID01IntroDialogMJQuestion>();
                boxChat.SetClick(false);
                //var box = boxChat.GetBox();
                //box.SetFollow(true);
                //SoundChannel clickGift = new SoundChannel(SoundChannel.PLAY_SOUND, dependency.ListenConfig.sfxClickGift, null, volume: 0.5f);
                //ObserverManager.TriggerEvent<SoundChannel>(clickGift);
                //bool canPlayAudio = false;
                //var skeleton = boxChat.GetSkeletonGraphicAnimation();
                //skeleton.AnimationState.SetAnimation(0, "Mo nap", false).Complete += (p) =>
                //{
                //    skeleton.gameObject.SetActive(false);
                //    box.gameObject.SetActive(false);
                //    canPlayAudio = true;
                //};
                //await UniTask.WaitUntil(() => canPlayAudio, cancellationToken: cancellationTokenSource.Token);
                await UniTask.Delay(dependency.ListenConfig.milisecondTimeDelay200, cancellationToken: cancellationTokenSource.Token);
                //max.AnimationState.SetAnimation(0, dependency.AnimTalk, true);
                var audioClip = boxChat.GetAudio();
                SoundChannel audio = new SoundChannel(SoundChannel.PLAY_SOUND_NEW_OBJECT, audioClip, null);
                ObserverManager.TriggerEvent<SoundChannel>(audio);
                await UniTask.Delay(Mathf.CeilToInt(audioClip.length) * 1000, cancellationToken: cancellationTokenSource.Token);
                // max.AnimationState.SetAnimation(0, dependency.AnimNormalToHappy, false);
                await UniTask.Delay(dependency.ListenConfig.milisecondTimeDelay500, cancellationToken: cancellationTokenSource.Token);
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
    public class BEID01IntroDialogMJListenStateData
    {
        public BEID01IntroDialogMJListenStateEventData EventData { get; set; }
    }
    public class BEID01IntroDialogMJListenStateEventData
    {
        public BEID01IntroDialogMJBoxChat BoxChat { get; set; }
    }
    public class BEID01IntroDialogMJListenStateDependency
    {
        public BEID01MJListenConfig ListenConfig { get; set; }
        public GameObject CharactersParent { get; set; }
        public SkeletonGraphic CharatersAnim { get; set; }
    }
}