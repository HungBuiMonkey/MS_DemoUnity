using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MJ5.BERV01FeedTheShark
{
    public class BERV01FTSPlayGuidingState : FSMState
    {
        private BERV01FTSPlayStateObjectDependency dependency;
        private CancellationTokenSource cts;
        private bool isGuiding = false;
        private BERV01FTSFish fishGuiding;
        private bool isStartGuiding = false;
        private BERV01FTSPlayStateData playStateData;
        private const float TIME_FADE = 0.5f;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            playStateData = (BERV01FTSPlayStateData)data;
            DoWork();
        }

        public override void SetUp(object data)
        {
            dependency = (BERV01FTSPlayStateObjectDependency)data;
        }

        private void DoWork()
        {
            cts = new CancellationTokenSource();
            if (playStateData.IsSkipGuiding)
            {
                ResetGuiding();
                if(playStateData.IsReplay) StartGuiding();
            } else
            {
                StartGuiding();
            }

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (isStartGuiding)
            {
                foreach(var fish in dependency.FishesPlay)
                {
                    if (BERV01FTSHandleData.IsClosestToCenter(fish.gameObject, dependency.CameraGame) && fish.FishData.isCorrect)
                    {
                        fishGuiding = fish;
                        isStartGuiding = false;
                        return;
                    }
                }
            }


        }

        public async void StartGuiding()
        {
            isGuiding = true;
            SoundChannel soundData;
            try
            {
                foreach (var fish in dependency.FishesPlay)
                {
                    if (fish.IsPlaying) return;
                }
                bool tscFadeDone = false;
                while (isGuiding)
                {
                    await UniTask.Delay(dependency.GuidingConfig.timeWaitStartGuiding, cancellationToken: cts.Token);
                    isStartGuiding = true;
                    await UniTask.WaitUntil(() => fishGuiding != null, cancellationToken: cts.Token);
                    dependency.ButtonSkipGuiding.GetComponent<CanvasGroup>().DOFade(0.75f, TIME_FADE);
                    dependency.ButtonSkipGuiding.Enable(true);
                    EnableMovingFish(false);
                    dependency.UiGuiding.transform.localScale = Vector3.one;
                    dependency.UiGuiding.transform.localRotation = new Quaternion(0f, fishGuiding.FishDirection == BERV01FishDirection.Left ? 0f : 180f, 0f, 0f);
                    dependency.UiGuiding.transform.position = fishGuiding.TextFish.transform.position;
                    await UniTask.Delay(dependency.GuidingConfig.timeDelay * 3, cancellationToken: cts.Token);
                    dependency.UiGuiding.DOFade(1f, TIME_FADE).SetEase(Ease.Linear).onComplete += () => { tscFadeDone = true; };
                    soundData = new SoundChannel(SoundChannel.PLAY_SOUND, dependency.GuidingConfig.sfxAppear);
                    ObserverManager.TriggerEvent<SoundChannel>(soundData);
                    await UniTask.WaitUntil(() => tscFadeDone, cancellationToken: cts.Token);
                    await UniTask.Delay(dependency.GuidingConfig.timeDelay * 2, cancellationToken: cts.Token);

                    for (int i = 0; i < 3; i++)
                    {
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND, dependency.GuidingConfig.sfxClick);
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        SetColorImage(dependency.HandLong, 0f);
                        SetColorImage(dependency.HandShort, 1f);
                        await UniTask.Delay(dependency.GuidingConfig.timeDelay * 2, cancellationToken: cts.Token); ;
                        soundData = new SoundChannel(SoundChannel.PLAY_SOUND, dependency.GuidingConfig.sfxUnClick);
                        ObserverManager.TriggerEvent<SoundChannel>(soundData);
                        SetColorImage(dependency.HandLong, 1f);
                        SetColorImage(dependency.HandShort, 0f);
                        await UniTask.Delay(dependency.GuidingConfig.timeDelay * 2, cancellationToken: cts.Token);
                    }

                    await UniTask.Delay(dependency.GuidingConfig.timeDelay, cancellationToken: cts.Token);
                    dependency.UiGuiding.DOFade(0f, TIME_FADE).SetEase(Ease.Linear).onComplete += () => { tscFadeDone = true; };
                    EnableMovingFish(true);
                    fishGuiding = null;
                    isStartGuiding = false;
                    dependency.ButtonSkipGuiding.GetComponent<CanvasGroup>().DOFade(0f, TIME_FADE);
                    dependency.ButtonSkipGuiding.Enable(false);
                }
            }
            catch (OperationCanceledException ex)
            {
                LogMe.Log("Lucanhtai ex: " + ex);
            }
        }
        public void ResetGuiding()
        {
            SoundManager.Instance.StopFx();
            if (BERV01FTSHandleData.CurrentIndexFishCorrect < BERV01FTSHandleData.MAX_FISH_CORRECT)
            {
                EnableMovingFish(true);
            } 
            fishGuiding = null;
            isGuiding = false;
            isStartGuiding = false;
            dependency.ButtonSkipGuiding.Enable(false);
            dependency.ButtonSkipGuiding.GetComponent<CanvasGroup>().DOFade(0f, 0.1f);
            dependency.UiGuiding.DOKill();
            dependency.UiGuiding.DOFade(0f, 0.1f);
            dependency.UiGuiding.transform.localScale = Vector3.zero;
        }
        private void EnableMovingFish(bool value)
        {
            foreach(var fish in dependency.FishesPlay)
            {
                if(!fish.IsPlaying) fish.EnableMovingFish(value);
            }
        }

        private void SetColorImage(Image image, float indexColor)
        {
            Color currentColor = image.color;
            currentColor.a = indexColor;
            image.color = currentColor;
        }

        public override void OnExit()
        {
            base.OnExit();
            ResetGuiding();
            cts?.Cancel();
            cts?.Dispose();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts?.Cancel();
            cts?.Dispose();
        }
    }
    public class BERV01FTSPlayStateData
    {
        public bool IsSkipGuiding { get; set; }
        public bool IsReplay { get; set; }
    }


    public class BERV01FTSPlayStateObjectDependency
    {
        public BERV01FTSGuidingConfig GuidingConfig { get; set; }
        public CanvasGroup UiGuiding { get; set; }
        public BaseButton ButtonSkipGuiding { get; set; }
        public List<BERV01FTSFish> FishesPlay { get; set; }
        public Image HandLong { get; set; }
        public Image HandShort { get; set; }
        public Camera CameraGame { get; set; }
    }
}