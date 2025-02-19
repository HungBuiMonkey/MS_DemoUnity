using Cysharp.Threading.Tasks;
using DG.Tweening;
using MonkeyBase.Observer;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace Monkey.MonkeyGo.BEPP01BlendingSound
{
    public class BEPP01BlendingSoundGuidingState : FSMState
    {
        private BEPP01BlendingSoundGuidingStateDataObjectDependency dependency;
        private CancellationTokenSource cts;

        private int timeDelayGuiding = 5000;

        public override void OnEnter(object data)
        {
            base.OnEnter(data);
            BEPP01BlendingSoundGuidingStateData guidingStateData = (BEPP01BlendingSoundGuidingStateData)data;
            DoWork(guidingStateData);
        }

        public override void SetUp(object data)
        {
            dependency = (BEPP01BlendingSoundGuidingStateDataObjectDependency)data;
        }


        private void DoWork(BEPP01BlendingSoundGuidingStateData data)
        {
            cts = new();
            dependency.ellieSkeleton.AnimationState.SetAnimation(0, dependency.ellieGuiding, true);
            foreach (var item in dependency.ListButtonAnswer)
            {
                item.GetComponent<BaseButton>().Enable(true);
            }
            LoopGuiding(data.audioWord);
        }
        private async void LoopGuiding(AudioClip audioW)
        {
            try
            {
                SoundChannel soundDataGuing;
                await UniTask.Delay(timeDelayGuiding, cancellationToken: cts.Token);

                bool isPlayingSound = false;
                soundDataGuing = new(SoundChannel.PLAY_SOUND, dependency.guidingDataConfig.audioGuiding, () => { isPlayingSound = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundDataGuing);
                await UniTask.WaitUntil(() => isPlayingSound, cancellationToken: cts.Token);
                isPlayingSound = false;
                soundDataGuing = new(SoundChannel.PLAY_SOUND, audioW, () => { isPlayingSound = true; });
                ObserverManager.TriggerEvent<SoundChannel>(soundDataGuing);
                await UniTask.WaitUntil(() => isPlayingSound, cancellationToken: cts.Token);
                LoopGuiding(audioW);
            }
            catch { }
        }
        public override void OnExit()
        {
            base.OnExit();
            cts?.Cancel();
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            cts.Cancel();
            cts.Dispose();
        }
        public override void OnApplicationPause(bool pause)
        {
            base.OnApplicationPause(pause);

            if (pause)
            {
                dependency.boxAnswer.GetComponent<RectTransform>().DOAnchorPosY(dependency.pointBoxAnswerAppear.anchoredPosition.y, 0f);
            }
            else
            {
                dependency.boxAnswer.GetComponent<RectTransform>().DOAnchorPosY(dependency.pointBoxAnswerAppear.anchoredPosition.y, 0f);
            }
        }
    }


    public class BEPP01BlendingSoundGuidingStateData
    {
        public AudioClip audioWord { set; get; }
    }

    public class BEPP01BlendingSoundGuidingStateDataObjectDependency
    {
        public SkeletonGraphic ellieSkeleton { get; set; }
        public BEPP01BlendingSoundGuidingConfig guidingDataConfig { get; set; }
        public List<Button> ListButtonAnswer { get; set; }
        public RectTransform boxAnswer { set; get; }
        public RectTransform pointBoxAnswerAppear { set; get; }
        public string ellieGuiding { set; get; }
    }
}